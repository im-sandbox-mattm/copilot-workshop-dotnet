// Lab 2 — Prompt Engineering Exercise
// =====================================
// Task: Use Copilot to write a US zip code validator function.
//
// SOLO (10 min):
//   1. Use Copilot with a VAGUE prompt to generate validateZip() below.
//   2. Uncomment the test calls at the bottom and run: node ZipCodeValidator.js
//   3. How many of the 4 cases pass? Try one follow-up prompt to fix a gap.
//
// PAIR (15 min):
//   1. Before prompting — write a spec together (return type, edge cases, errors).
//   2. Delete the function and use a CONSTRAINED prompt based on your spec.
//   3. Run the same 4 test cases. Compare results with your solo version.
//   4. Ask Copilot: "What edge cases does this function not cover?"
//
// RUN: node ZipCodeValidator.js
// =====================================

// --- Copilot generates your function here ---


// --- Test cases (uncomment after generating the function) ---
// console.log(validateZip("12345"));       // expected: true  (standard 5-digit)
// console.log(validateZip("00123"));       // expected: true  (leading zero)
// console.log(validateZip("12345-6789")); // expected: true  (ZIP+4 format)
// console.log(validateZip("1234"));        // expected: false (too short)
