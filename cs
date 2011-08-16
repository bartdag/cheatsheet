#!/usr/bin/env bash

CSDIR=~/".cs"

function usage {
    printf "Usage: %s: [-i] [-e] [-d csdir] cheatsheet [section]\n" $(basename $0) >&2
}

function showhelp {
    printf "Not implemented yet\n"
}

function checkcsdir {
    if [ ! -d "$CSDIR" ]
    then
        printf "The cheatsheet directory, ${CSDIR}, does not exist. "
        printf "Did you run cs -i ?\n"
        exit 1
    fi
}

function localinit {
    if mkdir "$CSDIR" > /dev/null 2>&1
    then
        printf "Cheatsheet directory initialized in ${CSDIR}\n"
    else
        printf "The cheasheet directory, ${CSDIR} could not be created.\n"
        exit 1
    fi
}

function gitinit {
    printf "Not implemented yet\n"

}

function hginit {
    printf "Not implemented yet\n"
}

function edit {
    checkcsdir
    local cheatpath="${CSDIR}/${1}"
    $EDITOR "${cheatpath}"
}

function gitcommit {
    printf "\n"
}

function hgcommit {
    printf "\n"
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

while getopts ':iegmhlsd:' OPTION
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
        gitcommit "${1}"
        hgcommit "${1}"
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
