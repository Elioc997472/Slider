import csv
import glob
import os
import re
import string

BASE_PATH = "./Localizations"

EXPECTED_HEADER = ["Path", "Orig", "Translation"]

# ---------- Ignore Rules ----------


SKIP_LANGUAGES = {
    "English",
}

SKIP_FILES = {
    "Credits_scene.csv",
}

PUNCTUATION_SET = set(string.punctuation + "—–…" + "01234567890")

RESOLUTION_REGEX = re.compile(
    r"^\s*\d{3,5}\s*[x×]\s*\d{3,5}\s*$",
    re.IGNORECASE
)

NUMBER_REGEX = re.compile(r"^\s*\d+(\.\d+)?\s*$")

VAR_REGEX = re.compile(r"<var>.+</var>")

def should_ignore(text: str) -> bool:
    if not text:
        return True

    stripped = text.strip()

    # Only punctuation
    if all(ch in PUNCTUATION_SET for ch in stripped):
        return True

    # Only numbers
    if NUMBER_REGEX.match(stripped):
        return True

    # Screen resolution (1920x1080, 1920 x 1080, 1920×1080)
    if RESOLUTION_REGEX.match(stripped):
        return True

    # Variable tags
    if VAR_REGEX.search(stripped):
        return True

    return False


# ---------- CSV Parsing ----------

def find_header_and_rows(csv_path):
    with open(csv_path, newline="", encoding="utf-8-sig") as f:
        rows = list(csv.reader(f))

    for i, row in enumerate(rows):
        if len(row) >= 3 and row[:3] == EXPECTED_HEADER:
            return i, rows

    return None, rows


def verify_csv_file(csv_path):
    issues = []

    header_index, rows = find_header_and_rows(csv_path)

    if header_index is None:
        print(f"⚠️  No valid header found in {csv_path}")
        return issues

    header = rows[header_index]
    col_index = {name: idx for idx, name in enumerate(header)}

    path_idx = col_index.get("Path")
    orig_idx = col_index.get("Orig")
    trans_idx = col_index.get("Translation")

    for row_num, row in enumerate(rows[header_index + 1:], start=header_index + 2):
        if len(row) <= max(orig_idx, trans_idx):
            continue

        orig = row[orig_idx].strip()
        translation = row[trans_idx].strip()
        path = row[path_idx].strip() if path_idx is not None else ""

        if should_ignore(orig):
            continue

        if orig == translation:
            issues.append({
                "file": csv_path,
                "row": row_num,
                "path": path,
                "text": orig
            })

    return issues


# ---------- Runner ----------

def main():
    all_issues = []

    csv_files = []

    for entry in os.listdir(BASE_PATH):
        lang_path = os.path.join(BASE_PATH, entry)

        if not os.path.isdir(lang_path):
            continue

        if entry in SKIP_LANGUAGES:
            continue

        for csv_file in glob.glob(os.path.join(lang_path, "*.csv")):
            if os.path.basename(csv_file) in SKIP_FILES:
                continue

            csv_files.append(csv_file)

    if not csv_files:
        print("No CSV files found.")
        return

    for csv_file in csv_files:
        all_issues.extend(verify_csv_file(csv_file))

    if not all_issues:
        print("✅ All localization files are valid. No identical Orig/Translation pairs found.")
        return

    print("❌ Localization issues found:\n")

    for issue in all_issues:
        print(
            f"File: {issue['file']}\n"
            f"Row: {issue['row']}\n"
            f"Path: {issue['path']}\n"
            f"Text: \"{issue['text']}\"\n"
            f"{'-'*50}"
        )

    print(f"\nTotal issues found: {len(all_issues)}")


if __name__ == "__main__":
    main()
