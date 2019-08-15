
<img src="https://raw.githubusercontent.com/mrbandler/ksc/master/Icons/logo-text.png" alt="ksc Icon Banner" width="150" height="110" style="display: flex; justify-content: center;"/>

# ksc - Kerboscript Compiler

[![pipeline status](https://gitlab.com/mrbandler/ksc/badges/master/pipeline.svg)](https://gitlab.com/mrbandler/ksc/commits/master) [![npm version](https://badge.fury.io/js/ksc.svg)](https://badge.fury.io/js/ksc) [![Donate with Bitcoin](https://en.cryptobadges.io/badge/micro/3KGsDx52prxWciBkfNJYBkXaTJ6GUURP2c)](https://en.cryptobadges.io/donate/3KGsDx52prxWciBkfNJYBkXaTJ6GUURP2c) [![Donate with Litecoin](https://en.cryptobadges.io/badge/micro/LcHsJH13A8PmHJQwpbWevGUebZwhWNMXgS)](https://en.cryptobadges.io/donate/LcHsJH13A8PmHJQwpbWevGUebZwhWNMXgS) [![Donate with Ethereum](https://en.cryptobadges.io/badge/micro/0xd6Ffc89Bc87f7dFdf0ef1aefF956634d4B7451c8)](https://en.cryptobadges.io/donate/0xd6Ffc89Bc87f7dFdf0ef1aefF956634d4B7451c8) [![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.me/mrbandler/)

**A command line tool to streamline work with larger Kerboscript projects.**

## Table Of Content

 1. [Idea and Use Case](#1-idea-and-use-case) 🤔
 2. [Installation](#2-installation) 💻
 3. [Usage](#3-usage) ⌨️
 4. [Bugs and Features](#4-bugs-and-features) 🐞💡
 5. [Buy me a coffee](#5-buy-me-a-coffee) ☕
 6. [License](#6-license) 📃

---

## 1. Idea and Use Case

This project started out with me working on my open source educational project called [*Project Artemis*](https://github.com/mrbandler/project-artemis) where I will try to completely automate a space program with [Kerbal Space Program](https://www.kerbalspaceprogram.com/) and the mod [kOS](http://ksp-kos.github.io/KOS_DOC/index.html). While writting my first few lines of Kerboscript it became very cumbersome and error prone to see if the written code is compilable without a lot of copying and running KSP. I just wanted to keep productive without a context switch.

This is the solution I came up with, a CLI for managing Kerboscript projects with features to compile, run and deploy Kerboscript with just a simple command, and even with your custom compile and deployment scripts written in entirely in Kerboscript! 

## 2. Installation

The CLI can easily be installed over [npm](https://www.npmjs.com/) (Node Package Manager). Which can be installed by installing [nodejs](https://nodejs.org/).

````shell
> npm install -g ksc
````

That's it, now you're up and running!

> **NOTE**
>
> The package is currently rather big as .NET Core 2.2 does not let me create a self-contained executable with tree shaking. .NET Core 3.0 has all these great features so I will switch to it once it's production ready.

## 3. Usage

**Coming soon...**

## 4. Bugs and Features

Please open a issue when you encounter any bugs 🐞 or if you have an idea for a additional feature 💡.

## 5. Buy me a coffee

If you like you can buy me a coffee:

[![Support via PayPal](https://cdn.rawgit.com/twolfson/paypal-github-button/1.0.0/dist/button.svg)](https://www.paypal.me/mrbandler/)

[![Donate with Bitcoin](https://en.cryptobadges.io/badge/big/3KGsDx52prxWciBkfNJYBkXaTJ6GUURP2c)](https://en.cryptobadges.io/donate/3KGsDx52prxWciBkfNJYBkXaTJ6GUURP2c)

[![Donate with Litecoin](https://en.cryptobadges.io/badge/big/LcHsJH13A8PmHJQwpbWevGUebZwhWNMXgS)](https://en.cryptobadges.io/donate/LcHsJH13A8PmHJQwpbWevGUebZwhWNMXgS)

[![Donate with Ethereum](https://en.cryptobadges.io/badge/big/0xd6Ffc89Bc87f7dFdf0ef1aefF956634d4B7451c8)](https://en.cryptobadges.io/donate/0xd6Ffc89Bc87f7dFdf0ef1aefF956634d4B7451c8)

---

## 6. License

MIT License

Copyright (c) 2019 mrbandler

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.