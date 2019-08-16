#!/usr/bin/env node

// Requires
const { spawn } = require("child_process");
const os = require("os");

// Constants.
const PLATFORM_WIN32 = "win32";
const PLATFORM_LINUX = "linux";
const PLATFORM_MACOS = "darwin";

const ARCH_X64 = "x64";
const ARCH_X86 = "x86";

const BIN_WIN32_X64 = __dirname + "/win-x64/ksc.exe";
const BIN_WIN32_X86 = __dirname + "/win-x86/ksc.exe";
const BIN_LINUX = __dirname + "/linux-x64/ksc";
const BIN_MACOS = __dirname + "/osx-x64/ksc";

// Returns the OS specific .NET binary based on the currently running win32 arch.
function getWinOSArchBin() {
    let bin = "";
    
    if (os.arch() === ARCH_X64) {
        bin = BIN_WIN32_X64;
    } else if (os.arch() === ARCH_X86) {
        bin = BIN_WIN32_X86;
    }

    return bin;
}

// Returns the OS specific .NET binary.
function getOSBin() {
    let bin = "";

    switch (os.platform()) {
        case PLATFORM_WIN32: bin = getWinOSArchBin();
            break;
        case PLATFORM_LINUX: bin = BIN_LINUX;
            break;
        case PLATFORM_MACOS: bin = BIN_MACOS;
            break;
    }

    return bin;
}

// Spawns the ksc process based on given arguments.
function spawnProcess(args) {
    let process = null;

    const bin = getOSBin();
    if (bin !== "" && bin !== null) {
        process = spawn(bin, args, {
            stdio: "inherit",
        });
    }

    return process;
}

// Cli main function.
function cli() {
    const args = process.argv.filter((a) => !a.includes("node") && !a.includes("cli.js"));
    spawnProcess(args);
}

cli();