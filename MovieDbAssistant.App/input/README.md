# /input

___

files in this folder are batch processed

## rules

:arrow_right: process files named:

- `<name>.json` : scrapper json output

- `<name>.txt` : query text

:point_right: files names starting with `-` are **NOT PROCESSED**

:point_right: files names starting with `!` lead to query cache bypass (query re-performed, cache replaced)

:arrow_right: publish builds in `<output_folder>/<name>/`
