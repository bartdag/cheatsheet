#!/usr/bin/env bash

CSDIR=~/".cs"

function usage {
    printf "Usage: %s: [-i] [-e] [-d csdir] cheatsheet [section]\n" $(basename $0) >&2
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

function edit {
    checkcsdir
    local cheatpath="${CSDIR}/${1}"
    $EDITOR "${cheatpath}"
}

function showcs {
    checkcsdir
    local cheat_path="${CSDIR}/${1}"

    if [ -f "${cheat_path}" ]
    then
        cat "${cheat_path}"
        printf "\n"
    else
        printf "The cheatsheet ${1} does not exist.\n"
        exit 1
    fi
}

function showsection {
    checkcsdir
    local cheat_path="${CSDIR}/${1}"
    if [ -f "${cheat_path}" ]
    then
        awk "/^\\[.+\\]/ { flag = 0} 
{ if (flag == 1) { print \$0 } }
/^\\[${2}\\]/ { flag = 1}" "${cheat_path}"
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
    local cheat_path="${CSDIR}/${1}"
    if [ -f "${cheat_path}" ]
    then
        awk '/^\[.+\]/' "${cheat_path}"
    else
        printf "The cheatsheet ${1} does not exist.\n"
        exit 1
    fi
}

# Transform long options into short ones
for ARG in $*
do
    delim=""
    case "$ARG" in
        --init) ARGS="${ARGS}-i ";;
        --edit) ARGS="${ARGS}-e ";;
        --csdir) ARGS="${ARGS}-d ";;
        --list) ARGS="${ARGS}-l ";;
        --list-sections) ARGS="${ARGS}-s ";;
        # pass through anything else
        *) [[ "${ARG:0:1}" == "-" ]] || delim="\""
            ARGS="${ARGS}${delim}${ARG}${delim} ";;
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

while getopts ':ielsd:' OPTION
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

if [ "$csdirflag" ]
then
    CSDIR="$csdirvar"
fi

if [ "$initflag" ]
then
	localinit
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
        print "Not enough argument. Need a cheatsheet name.\n"
        exit 2
    fi
fi

if [ "$editflag" ]
then
	if [ $# -ge 1 ]
    then
        edit "${1}"
        exit 0
    else
        print "Not enough argument. Need a cheatsheet name.\n"
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
