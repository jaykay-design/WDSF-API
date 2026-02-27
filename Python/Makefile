all: check

check: style test

style:
	ruff format
	ruff check --fix
	ruff check

test:
	pytest .