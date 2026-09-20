import { validateRepository } from "./repository-integrity.mjs";

const result = validateRepository();
if (result.errors.length) {
  console.error("Repository integrity FAILED");
  for (const error of result.errors) console.error("- " + error);
  process.exitCode = 1;
} else {
  console.log("Repository integrity PASSED");
  console.log(JSON.stringify(result.summary, null, 2));
}
