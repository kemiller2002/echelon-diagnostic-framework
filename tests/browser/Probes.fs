/// In-page probes. Each probe is a pure JavaScript function evaluated in the page
/// that returns findings as data; the F# side decides what is a failure.
module EdfSite.Probes

/// Containers whose rendered content must stay inside their own box: Forma's
/// rounded and bordered components plus the shell regions that hold them.
[<Literal>]
let ContainerSelector =
    ".ef-card, .ef-callout, .ef-badge, .ef-button, .ef-site-nav__link, .ef-facts__item, \
     .ef-index__item, .ef-cta, .ef-section-heading, .ef-hero, .ef-site-header__inner, \
     .ef-site-footer__inner, .ef-site__main blockquote, .ef-steps > li"

/// Page overflow, viewport escapes, content wider than its container, and text
/// whose glyph boxes cross a container's visible edge.
let geometry =
    """
    (selector) => {
      const vw = document.documentElement.clientWidth;
      const label = el => el.tagName.toLowerCase() + (el.id ? '#' + el.id : '') + [...el.classList].map(c => '.' + c).join('');
      const visible = el => { const s = getComputedStyle(el); const r = el.getBoundingClientRect(); return s.display !== 'none' && s.visibility !== 'hidden' && r.width > 0 && r.height > 0; };
      const findings = [];
      const docW = document.documentElement.scrollWidth;
      if (docW > vw + 1) findings.push({ kind: 'page-overflow', target: 'html', detail: `document ${docW}px wide in a ${vw}px viewport` });
      const textRects = el => {
        const walker = document.createTreeWalker(el, NodeFilter.SHOW_TEXT, { acceptNode: n => n.textContent.trim() ? NodeFilter.FILTER_ACCEPT : NodeFilter.FILTER_REJECT });
        const rects = [];
        for (let n = walker.nextNode(); n; n = walker.nextNode()) {
          if (!visible(n.parentElement)) continue;
          const range = document.createRange(); range.selectNodeContents(n);
          rects.push(...[...range.getClientRects()].filter(r => r.width > 0));
        }
        return rects;
      };
      [...document.querySelectorAll(selector)].filter(visible).forEach(c => {
        const s = getComputedStyle(c);
        const r = c.getBoundingClientRect();
        if (r.right > vw + 1 || r.left < -1) findings.push({ kind: 'viewport-escape', target: label(c), detail: `left ${Math.round(r.left)} right ${Math.round(r.right)} viewport ${vw}` });
        if (s.overflowX === 'visible' && c.scrollWidth > c.clientWidth + 1) findings.push({ kind: 'content-overflow', target: label(c), detail: `content ${c.scrollWidth}px in a ${c.clientWidth}px box` });
        const bl = parseFloat(s.borderLeftWidth) || 0, br = parseFloat(s.borderRightWidth) || 0;
        const escaped = textRects(c).filter(t => t.left < r.left + bl - 1 || t.right > r.right - br + 1);
        if (escaped.length) findings.push({ kind: 'text-escape', target: label(c), detail: `${escaped.length} text box(es) cross the container edge` });
      });
      return findings;
    }
    """

/// Pointer targets: WCAG 2.2 2.5.8 (24px, inline links in text exempt) and Forma's
/// 44px block target for navigation links and buttons.
let targets =
    """
    () => {
      const label = el => el.tagName.toLowerCase() + [...el.classList].map(c => '.' + c).join('') + ' "' + el.textContent.trim().slice(0, 40) + '"';
      const inText = el => getComputedStyle(el).display === 'inline' && el.closest('p, dd, li:not(.ef-site-nav__list > li), blockquote, h1, h2, h3');
      const findings = [];
      document.querySelectorAll('a[href], button').forEach(el => {
        const r = el.getBoundingClientRect();
        if (r.width === 0 || r.height === 0 || inText(el)) return;
        if (el.matches('.ef-card__link')) return; // the stretched link makes the whole card the target
        if (r.width < 24 || r.height < 24) findings.push({ kind: 'target-size', target: label(el), detail: `${Math.round(r.width)}x${Math.round(r.height)}px < 24px` });
        if (el.matches('.ef-site-nav__link, .ef-button') && r.height < 44) findings.push({ kind: 'target-size', target: label(el), detail: `height ${Math.round(r.height)}px < 44px` });
      });
      return findings;
    }
    """

/// Every primary navigation link is visible and inside the viewport at every width.
let navigation =
    """
    () => {
      const vw = document.documentElement.clientWidth;
      const links = [...document.querySelectorAll('.ef-site-nav__link')];
      const findings = links.flatMap(a => {
        const s = getComputedStyle(a); const r = a.getBoundingClientRect();
        const shown = s.display !== 'none' && s.visibility !== 'hidden' && r.width > 0 && r.height > 0;
        return shown && r.left >= -1 && r.right <= vw + 1 ? [] : [{ kind: 'navigation-hidden', target: a.getAttribute('href'), detail: `rect ${Math.round(r.left)}..${Math.round(r.right)} in ${vw}` }];
      });
      return links.length === 5 ? findings : [...findings, { kind: 'navigation-missing', target: 'nav', detail: `${links.length} primary links, expected 5` }];
    }
    """

/// Describes the focused element and whether it paints a visible indicator.
let focused =
    """
    () => {
      const el = document.activeElement;
      if (!el || el === document.body) return null;
      const ring = e => { const s = getComputedStyle(e); return (s.outlineStyle !== 'none' && parseFloat(s.outlineWidth) > 0) || (s.boxShadow && s.boxShadow !== 'none'); };
      const holder = el.matches('.ef-card__link') ? el.closest('.ef-card') : el;
      const r = el.getBoundingClientRect();
      return {
        target: el.tagName.toLowerCase() + [...el.classList].map(c => '.' + c).join('') + ' "' + el.textContent.trim().slice(0, 40) + '"',
        visibleIndicator: !!ring(holder),
        inViewport: r.bottom > 0 && r.top < innerHeight && r.right > 0 && r.left < innerWidth,
        isSkipLink: el.matches('.ef-skip-link')
      };
    }
    """

/// Appends long unbroken URLs, identifiers, labels and headings to real content.
let stress =
    """
    () => {
      const url = 'https://github.com/kemiller2002/echelon-diagnostic-framework/blob/main/research/registries/hypothesis-registry.json#HY-EDF-001-provenance';
      const token = 'EX-EDF-001-structural-value-benchmark-preregistration-sha256-2f9ceeb6ea99a54f6cf4ffa03e3a69fd';
      const add = (selector, text, limit = 2) => [...document.querySelectorAll(selector)].slice(0, limit).forEach(el => el.append(' ' + text));
      add('h1', token, 1);
      add('h2', token);
      add('.ef-card__title', url);
      add('.ef-card p', url);
      add('.ef-badge', token);
      add('.ef-callout p', url);
      add('.ef-index__title', token);
      add('.ef-facts dd', url);
      add('.ef-site__main blockquote p', url);
      add('.ef-steps > li', url);
      add('.ef-site-footer__note span', url);
      add('.ef-button', 'with the complete EDF–0 worksheet and its supporting notes');
      add('.ef-site-nav__link', 'and supporting material', 5);
      add('.ef-site-header__name', '— working draft specification v0.3', 1);
    }
    """
