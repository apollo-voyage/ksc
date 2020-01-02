---
sidebarDepth: 2
---

# Compilation

## Syntax

### Single Kerboscript

```bash
> ksc compile -i <file> [-o <file>]
```

### Kerboscript Source Directory

```bash
> ksc compile -i <dir> [-o <dir>]
```

### ksconfig.json

```bash
> ksc compile [-v <volume>]
```

## The Compile Verb

To compile **Kerboscripts** you can use the `compile` verb.

To give the user the best possbile flexibiliy the `compile` verb can be used in three different scenarios.

Let's go over all scenarios.

## Compiling a Single Kerboscript

The most simple and basic use case for the `compile` verb is to compile a single **Kerboscript** file.

Let's say we have the following directory structure:

```
project-artemis/
| - boot.ks
```

Just a simple directory with a single **Kerboscript** file in it. Let's compile it.

To compile the file simply execute the following command in the `project-artemis` directory:

```bash
> ksc compile -i boot.ks
```

This will create the following new structure:

```
project-artemis/
| - boot.ks
| - boot.ksm
```

It just compiled the source code into a `*.ksm` output file which is created right next to the source file.

### Configuring Output

Great! Now that we seen how to compile a single **Kerboscript** you may be asking, "But wait, can I specify where to compile to so that my source directory doesn't get too polluted?".

Well you are in luck, there is a option for that. Let's take it for a spin.

We will again have the follwing directory structure:

```
project-artemis/
| - boot.ks
```

Now, let's recompile the `boot.ks` file put instead of creating the output file right next to it, we will create it in a seperate sub-directory.

```bash
> ksc compile -i boot.ks -o ./out/boot.ksm
```

::: warning
**NOTE**

It is possible to omit the `*.ksm` extension for the output file, if it is left out it will be put in automatically.
:::

Let's look at the created result:

```
project-artemis/
| - boot.ks
| - out/
|   | - boot.ksm
```

That's it this is as simple as it gets for compiling a **Kerboscript** outside of Kerbal Space Program.

## Compiling a Kerboscript Source Directory

Alright, now that we know how to compile a single **Kerboscript** file we can move on to try to compile a complete set of `*.ks` files.

Let's say we have the following directory structure:

```
project-artemis/
| - src/
|   | - boot/
|   |   | - boot.ks
|   | - main.ks
|   | - console.ks
|   | - orbit.ks
```

## Compiling with ksconfig.json