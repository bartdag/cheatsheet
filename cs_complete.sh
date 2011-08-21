#!/usr/bin/env bash

CSDIR=~/".cs"

function _cscomplete_() {
    local cmd="${1##*/}"
    local word="${COMP_WORDS[COMP_CWORD]}"
    local line="${COMP_LINE}"
    
    IFS=':'
    local cheats=''

    for f in "${CSDIR}"/*;
    do
        cheats="${cheats}:$(basename $f)"
    done

    COMPREPLY=($(compgen -W "${cheats}" "${word}"))
}

complete -F _cscomplete_ cs
