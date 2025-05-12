# Publish Folder

This folder is used for publishing the TextTv.Cli application as a self-contained, single executable file. 

All contents of this folder (except this README.md) are excluded from version control via .gitignore.

## Published Application

When the application is published using the command specified in the main README.md, it will create:

- A single executable binary `texttv` (Linux) 
- Any required runtime configuration files

## Usage

After publishing, you can run the application directly with:

```bash
./texttv 100
```

Where `100` is the Text TV page number you want to view.
