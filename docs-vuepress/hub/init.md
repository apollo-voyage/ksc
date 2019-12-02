---
sidebarDepth: 2
---

# Let's Start A New Project

::: warning
Altough `ksc` can be used to compile a single **Kerboscript** file or a complete **source directory**, the best way to use `ksc` is through the build in project management functionality.
:::

## Syntax

```bash
> ksc init [project-name] [project-path] [options]
```

## The Init Verb

To start a new `ksc` project in your current directory simply execute the following command:

```bash
> ksc init project-artemis
```

::: warning
**NOTE**

It is possible to omit the `project-artemis` here, as the wizard will ask for it.

If the option to skip the wizard is used, the name of the directory the project will be created in will be used as the project name.
:::

This will ask your some basic questions about your projects and will then create the following directory structure:

```
project-artemis/
| - boot/
|   | - boot.ks
| - scripts/
|   | - compile.ks
|   | - deploy.ks
| - src/
| - ksconfig.js
```

### Directory Structure Breakdown

Let's start with the most important file, the `ksconfig.json` which is the [**Project Definition File**](/hub/init.html#project-definition-file).

This file holds all the information `ksc` asked you about when your created the project, like **name**, **description**, **archive** (Kerbal Space Program installation), **volumes** and **scripts**. For a detailed description read the next main section.

Furthermore, it created some default directories with some files in it, let's take a look at those.

#### `boot` Directory

The `boot` directory resembles the `boot` directory used by kOS to determin boot-files for a rocket launch. Find a more detailed guide on boot-files in the [kOS Documentation](https://ksp-kos.github.io/KOS/general/volumes.html?#special-handling-of-files-in-the-boot-directory).

#### `src` Directory

The `src` directory is intended to hold the bulk of the projects **Kerboscript**.
That's the reason it's empty.

#### `scripts` Directory

Finally the `scripts` directory is used to hold only development or project specific **Kerboscript** files that can be used to automate tasks with the use of **Kerboscript** only.

Please refer to the [Project Scripts & Execution](/hub/run.html) section for a detailed guide on how to leverage this functionality.

### Addtional Parameters & Options

The `init` verb has some additional options which might be handy.

#### Project Path

Now, let's think about the moment someone might want to create a new project, that could be literally anywhere (in the a directory structure I mean). So, it would be very handy if someone could create a project from anywhere. The `Project Path` will let you do that.

Let's look at a example.

```
root/
| - home/
|   | - etc/
| - dev/
|   | - projects/
```

Let's say you are currently in the `root/home/etc` directory and you suddenly have the urge to create a **Kerboscript** project, us this:

```bash
> ksc init project-artemis ../../dev/projects/
```
This will create a now directory inside your `projects/` directory, which is named after your project name.

There you go you just created a **Kerboscript** project from very far away!

#### Use Defaults

The last option for the `init` verb is a simple switch to disable the interactive project creation and just use the default settings.

If we continue with the example above a command would look something like this:

```bash
> ksc init project-artemis ../../dev/projects/ -y
```

## Project Definition File

The **Project Definition File** is the heart of every **Kerboscript** project created with `ksc`. 

Let's dive a little deeper on its contents.

### Basic Information

The basic information of a projects `ksconfig.json` contains the **name**, **description** and the **archive** of a project.

```json
{
    "name": "project-artemis",
    "description": "To the moon and to stay!",
    "archive": "C:/Program Files (x86)/Steam/steamapps/common/Kerbal Space Program/Ships/Script",
    ...
}
```

### Volumes

`Volumes` in a projects `ksconfig.json` are used to define volumes for the `ksc` simulated volume system for **Kerboscript** execution. This provides the ability to write custom scripts in **Kerboscript**  and to support a specific development style. For more information on this head to the [Project Scripts & Execution](/hub/run/) section of the documentation.

```json
{
    ...
    "volumes": [
        {
            "index": 1,
            "name": "boot",
            "path": "./boot",
            "output": "./dist/boot",
            "deploy": "./boot"
        },
        {
            "index": 2,
            "name": "kerboscript-project",
            "path": "./src",
            "output": "./dist",
            "deploy": "."
        }
    ],
    ...
}
```

This is the default configuration of `volumes` in a `ksc` project.
It adds one for the `boot` section and one for all of the other **Kerboscripts** of the project.

Additional `volumes` can be added to split the code into logical groups. This comes in handy when you one wants to deploy a specific part of a project, as the `deploy` verb takes a `volume` as a option. See [Deployment](/hub/deploy/) for a detailed guide. 

A `Volume` has a specific structure let's look at each property and see what they do.

#### Index

Specifies the `index` of a `volume`.

The `index` will primarily be used as a option for various `ksc` verbs it can also be used to identify the `volume` in custom project scripts.

::: danger
**DANGER ZONE**

Never use **0** as a `volume`'s `index` as this `index` is reserved for the `archive` volume.

See [kOS Documentation](https://ksp-kos.github.io/KOS/general/volumes.html?#archive).
:::

#### Name

Specifies the `name` of a `volume`.

The `name` will primarily be used as a option for various `ksc` verbs it can also be used to identify the `volume` in custom project scripts.

#### Path

Specifies the directory location of the `volume`'s `path` relative to the project root.

#### Output

Specifies the directory location of the `volume`'s `output` or `dist` path relative to the project root.

#### Deploy

Specifies the directory location of the `volume`'s `deploy` path relative to the specified `archive` property of the project.

Example:

If the `deploy` path is set to `./boot` this will correspond to a complete path of `C:/Program Files (x86)/Steam/steamapps/common/Kerbal Space Program/Ships/Scripts/boot`.

::: warning
**NOTE**

The `dot` will be replaced by the `archive` path.
:::

### Scripts

`Scripts` in a projects `ksconfig.json` are used to define automation and/or utility scripts that can either be any arbitrary CLI tool or the `ksc`s `run` verb (you can find more information amout the `run` verb in the section [Project Scripts & Execution](/hub/run.html)).

If you familiar with [node](https://nodejs.org/) it is very similar to the `scripts` section in a `package.json`.

```json
{
    ...
    "scripts": [
        {
            "name": "compile",
            "content": "ksc run ./scripts/compile.ks"
        },
        {
            "name": "deploy",
            "content": "ksc run compile && ksc run ./scripts/deploy.ks"
        },
    ]
}
```

This is the default configuration of `scripts` in a `ksc` project.
Out of the box `ksc` comes with pre-written compilation and deployment scripts written in **Kerboscript** that are executed with `ksc run`.

But you can also choose to write [node](https://nodejs.org/) scripts and execute them here with the `node` CLI.

Additional `scripts` can be added to fullfill all your need for automation.

A `Script` has a specific structure let's look at each property and see what they do.

#### Name

Specifies the `name` of the `script` that can be referenced via [`run` verb](/hub/run.html) to execute it.

#### Content

Specifies the `content` of the `script` contains the commands to be executed.

## That's it!

Great stuff, now we have a `ksc` project created. 

Now, let's write some scripts and compile them!