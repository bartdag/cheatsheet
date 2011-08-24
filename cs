#!/usr/bin/env bash

CSDIR=~/".cs"

VERSION='0.1'

function usage {
    printf "Usage: %s: [-i] [-e] [-d csdir] cheatsheet [section]\n" $(basename $0) >&2
}

function showhelp {
    printf "Usage: cs [options] cheatsheet [section]\n\n"
    printf "Display a cheatsheet on the standard output.\n" 
    printf "If a section is specified, only the section is displayed.\n"
    printf "\nCheatsheet Format:\n"
    printf "A cheatsheet should be text file. A section title should be on\n"
    printf "a new line, between square brackets, e.g., [mysection]\n"
    printf "\nOptions:\n"
    printf "  -i,--init          Initialize the cheatsheet repository.\n"
    printf '  -e,--edit          Edit the specified cheatsheet with $EDITOR or vi.\n'
    printf "  -l,--list          List the cheatsheets.\n"
    printf "  -s,--list-sections List the sections in a cheatsheet.\n"
    printf "  -d,--csdir  CSDIR  Use CSDIR as the cheatsheet repository directory.\n"
    printf "  -g,--git           Initialize a git repository (with --init)\n"
    printf "  -m,--hg            Initialize a mercurial repository (with --init)\n"
    printf "  -h,--help          Display this help screen.\n"
    printf "  -v,--version       Display the version number.\n"
    printf "\nAbout Git and Mercurial integration:\n"
    printf "If a repository is initialized with --init --git or --init --hg,\n"
    printf "changes to a cheatsheet with --edit will be automatically committed.\n"
    printf "It is the responsibility of the user to clone an existing repository\n"
    printf "and to push the repository to a remote one, if needed.\n"

}

function checkcsdir {
    if [ ! -d "${CSDIR}" ]
    then
        printf "The cheatsheet directory, ${CSDIR}, does not exist. "
        printf "Did you run cs -i ?\n"
        exit 1
    fi
}

function localinit {
    if mkdir "${CSDIR}" > /dev/null 2>&1
    then
        printf "Cheatsheet directory initialized in ${CSDIR}\n"
    else
        printf "The cheasheet directory, ${CSDIR} could not be created.\n"
        exit 1
    fi
}

function gitinit {
    checkcsdir
    # No need to check anything as git is verbose enough
    if ! git init "${CSDIR}"
    then
        exit 1
    fi
}

function hginit {
    checkcsdir
    if hg init "${CSDIR}"
    then
        printf "Hg repository initialized in ${CSDIR}\n"
    else
        printf "Error while initializing Hg repository in ${CSDIR}\n"
        exit 1
    fi
}

function edit {
    checkcsdir
    local cheatpath="${CSDIR}/${1}"
    if [ "${EDITOR}" ]
    then
        $EDITOR "${cheatpath}"
    else
        vi "${cheatpath}"
    fi
}

function gitcommit {
    if ! git --git-dir="${CSDIR}/.git" --work-tree="${CSDIR}" add "${CSDIR}/${1}" > /dev/null 2>&1
    then
        printf "Could not add ${1} to git repository\n"
        exit 1
    fi

    if git --git-dir="${CSDIR}/.git" --work-tree="${CSDIR}" commit -m "Edited ${1}" > /dev/null 2>&1
    then
        printf "Changes to ${1} committed.\n"
    else
        printf "An error occurred while committing ${CSDIR}/${1}\n"
    fi
}

function hgcommit {
    if ! hg add "${CSDIR}/${1}"
    then
        printf "Could not add ${1} to hg repository\n"
        exit 1
    fi

    if hg -R "${CSDIR}" commit -m "Edited ${1}" > /dev/null 2>&1
    then
        printf "Changes to ${1} committed.\n"
    else
        printf "An error occurred while committing ${CSDIR}/${1}\n"
    fi
}

function showcs {
    checkcsdir
    local cheatpath="${CSDIR}/${1}"

    if [ -f "${cheatpath}" ]
    then
        cat "${cheatpath}"
        printf "\n"
    else
        printf "The cheatsheet ${1} does not exist.\n"
        exit 1
    fi
}

function showsection {
    checkcsdir
    local cheatpath="${CSDIR}/${1}"
    if [ -f "${cheatpath}" ]
    then
        awk "/^\\[.+\\]/ { flag = 0} 
{ if (flag == 1) { print \$0 } }
/^\\[${2}\\]/ { flag = 1}" "${cheatpath}"
    else
        printf "The cheatsheet ${1} does not exist.\n"
        exit 1
    fi
}

function listcs {
    checkcsdir
    ls "${CSDIR}"
}

function listsections {
    checkcsdir
    local cheatpath="${CSDIR}/${1}"
    if [ -f "${cheatpath}" ]
    then
        awk '/^\[.+\]/ {print substr( $0, 2, length($0) - 2)}' "${cheatpath}"
    else
        printf "The cheatsheet ${1} does not exist.\n"
        exit 1
    fi
}

# Transform long options into short ones
for arg in $*
do
    delim=""
    case "$arg" in
        --init) ARGS="${ARGS}-i ";;
        --edit) ARGS="${ARGS}-e ";;
        --csdir) ARGS="${ARGS}-d ";;
        --list) ARGS="${ARGS}-l ";;
        --list-sections) ARGS="${ARGS}-s ";;
        --git) ARGS="${ARGS}-g ";;
        --hg) ARGS="${ARGS}-m ";;
        --help) ARGS="${ARGS}-h ";;
        --version) ARGS="${ARGS}-v ";;
        # pass through anything else
        *) [[ "${arg:0:1}" == "-" ]] || delim="\""
            ARGS="${ARGS}${delim}${arg}${delim} ";;
    esac
done

eval set -- $ARGS

# Init flags
initflag=
editflag=
csdirflag=
listflag=
listsectionsflag=
csdirvar=
gitflag=
hgflag=
helpflag=
versionflag=

while getopts ':iegmhvlsd:' OPTION
do
	case $OPTION in
	i) 	    initflag=1
			;;
	e) 	    editflag=1
			;;
	d)      csdirflag=1
	        csdirvar="$OPTARG"
	        ;;
	l)      listflag=1
	        ;;
    s)
            listsectionsflag=1
            ;;
    g)      gitflag=1
            ;;
    m)      hgflag=1
            ;;
    h)      helpflag=1
            ;;
    v)      versionflag=1
            ;;
	?) 	    printf "unknown option: -%s\n" $OPTARG	
	        usage
	        exit 2
			;;
	\:)     printf "argument missing from -%s option\n" $OPTARG
	        usage
	        exit 2
	esac
done
shift $(($OPTIND -1))

if [ "$helpflag" ]
then
	showhelp
	exit 0
fi

if [ "$versionflag" ]
then
    printf "cheatsheet version $VERSION\n"
    exit 0
fi

if [ "$csdirflag" ]
then
    CSDIR="$csdirvar"
fi

if [ "$initflag" ]
then
	localinit
	if [ "$gitflag" ]
    then
        gitinit
    elif [ "$hgflag" ]
    then
        hginit
    fi
	exit 0
fi

if [ "$listflag" ]
then
    listcs
    exit 0
fi

if [ "$listsectionsflag" ]
then
    if [ $# -ge 1 ]
    then
        listsections "${1}"
        exit 0
    else
        printf "Not enough argument. Need a cheatsheet name.\n"
        exit 2
    fi
fi

if [ "$editflag" ]
then
	if [ $# -ge 1 ]
    then
        edit "${1}"
        if [ -d "${CSDIR}/.git" ]
        then
            gitcommit "${1}"
        elif [ -d "${CSDIR}/.hg" ]
        then
            hgcommit "${1}"
        fi
        exit 0
    else
        printf "Not enough argument. Need a cheatsheet name.\n"
        exit 2
    fi
fi

if [ $# -lt 1 ]
then
    printf "Not enough argument. Need a cheatsheet name.\n"
    usage
    exit 2
elif [ $# -eq 1 ]
then
    showcs "${1}"
else
    cs="${1}"
    shift
    section="${*}"
    showsection "${cs}" "${section}"
fi

exit 0
