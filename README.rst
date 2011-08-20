cheatsheet - A bash script to manage your personal cheatsheets
==============================================================

:Authors:
  Barthelemy Dagenais
:Version: 0.1

It seems I can never remember the subtleties of git reset, the difference
between markdown and restructuredText lists and the useful flags of the find
command. I always end up using duckduckgo or google to find these answers,
even though I know that (1) I used these commands before, (2) I probably wrote
down this information somewhere.

cs, the ultimate cheatsheet management bash script comes to the rescue. At
least, it came to my rescue.

One last note, the script tries to prevent some goofs but if you pass tons of
flags to the script and hope for the best, you are likely to be in for the
worst :-)


Requirements
------------

You need bash and awk.


Installation
------------

Place the script on your $PATH. Make sure that $EDITOR is set to your favorite
editor.


Usage
-----

::

    Usage: cs [options] cheatsheet [section]

    Display a cheatsheet on the standard output.
    If a section is specified, only the section is displayed.

    Cheatsheet Format:
    A cheatsheet should be text file. A section title should be on
    a new line, between square brackets, e.g., [mysection]

    Options:
    -i,--init          Initialize the cheatsheet repository.
    -e,--edit          Edit the specified cheatsheet with $EDITOR or vi.
    -l,--list          List the cheatsheets.
    -s,--list-sections List the sections in a cheatsheet.
    -d,--csdir  CSDIR  Use CSDIR as the cheatsheet repository directory.
    -g,--git           Initialize a git repository (with --init)
    -m,--hg            Initialize a mercurial repository (with --init)
    -h,--help          Display this help screen.

    About Git and Mercurial integration:
    If a repository is initialized with --init --git or --init --hg,
    changes to a cheatsheet with --edit will be automatically committed.
    It is the responsibility of the user to clone an existing repository
    and to push the repository to a remote one, if needed.cs --init


Status of the project
---------------------

All commands displayed in the usage are complete.


Todo
----

#. Add bash completion.


License
-------

This software is licensed under the `New BSD License`. See the `LICENSE` file
in the for the full license text.
