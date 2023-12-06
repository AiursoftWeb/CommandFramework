# Aiursoft CommandFramework

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/aiursoft/commandframework/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/aiursoft/commandframework/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/pipelines)
[![NuGet version (Aiursoft.CommandFramework)](https://img.shields.io/nuget/v/Aiursoft.CommandFramework.svg)](https://www.nuget.org/packages/Aiursoft.CommandFramework/)
[![ManHours](https://manhours.aiursoft.cn/r/gitlab.aiursoft.cn/aiursoft/commandframework.svg)](https://gitlab.aiursoft.cn/aiursoft/commandframework/-/commits/master?ref_type=heads)

Aiursoft CommandFramework is a framework for building command line tools.

* Auto argument parsing
* Auto help page generation
* Auto version page generation

With this framework, you can build a command line tool with just a few lines of code.

Example project it built:

* [Nuget Ninja](https://gitlab.aiursoft.cn/aiursoft/nugetninja)
* [DotDownload](https://gitlab.aiursoft.cn/aiursoft/dotdownload)
* [Parser](https://gitlab.aiursoft.cn/anduin/parser)
* [HappyRecorder](https://gitlab.aiursoft.cn/anduin/happyrecorder)
* [Dotlang](https://gitlab.aiursoft.cn/aiursoft/dotlang)
* [Ni Bot](https://gitlab.aiursoft.cn/aiursoft/ni-bot)
* [IPMI Controller](https://gitlab.aiursoft.cn/aiursoft/ipmicontroller)

```bash
C:\workspace> ninja.exe

Description:
  Nuget Ninja, a tool for detecting dependencies of .NET projects.

Usage:
  Microsoft.NugetNinja [command] [options]

Options:
  -p, --path <path> (REQUIRED)   Path of the projects to be changed.
  --nuget-server <nuget-server>  If you want to use a customized nuget server instead of the official nuget.org, 
  --token <token>                The PAT token which has privilege to access the nuget server.
  -d, --dry-run                  Preview changes without actually making them
  -v, --verbose                  Show detailed log
  -?, -h, --help                 Show help and usage information

Commands:
  all, all-officials  The command to run all officially supported features.
  remove-deprecated   The command to replace all deprecated packages to new packages.
  upgrade-pkg         The command to upgrade all package references to possible latest and avoid conflicts.
  clean-pkg           The command to clean up possible useless package references.
  clean-prj           The command to clean up possible useless project references.
  ```

## Why this project?

Command-line applications are a great way to automate repetitive tasks or even to be your own productivity tool. But building a command-line application in .NET is not easy. You need to parse the arguments, generate help pages, and so on. This project is designed to help you build a command-line application with just a few lines of code.

## Installation

Run the following command to install `Aiursoft.CommandFramework` to your project from [nuget.org](https://www.nuget.org/packages/Aiursoft.CommandFramework/):

To use it, you need to decide between:

* [Single command app](./docs/single_command.md)
* [Nested command app](./docs/nested_commands.md)

## Download a real sample project

If you want to explore a real project built with this framework, please check [Happy Recorder](https://gitlab.aiursoft.cn/anduin/HappyRecorder) as an example.

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.
