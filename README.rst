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

    cs --init
    cs --edit git           # open the git cheatsheet in an editor
    cs git                  # displays git cheatsheet
    cs git reset            # displays reset section in git cheatsheet
    cs --list               # list all cheatsheets
    cs --list-sections git  # list all sections in git cheatsheet


Status of the project
---------------------

Initial work only. Children may cry because of bugs.


Todo
----

#. Add -h --help flag.
#. Use a proper naming convention.
#. Add git and hg support.
#. Add bash completion. Seriously.


License
-------

This software is licensed under the `New BSD License`. See the `LICENSE` file
in the for the full license text.
