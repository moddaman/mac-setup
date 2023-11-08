{\rtf1\ansi\ansicpg1252\cocoartf2709
\cocoatextscaling0\cocoaplatform0{\fonttbl\f0\fnil\fcharset0 Menlo-Regular;}
{\colortbl;\red255\green255\blue255;\red89\green138\blue67;\red24\green24\blue24;\red193\green193\blue193;
\red70\green137\blue204;\red140\green211\blue254;\red202\green202\blue202;\red194\green126\blue101;\red212\green214\blue154;
}
{\*\expandedcolortbl;;\cssrgb\c41569\c60000\c33333;\cssrgb\c12157\c12157\c12157;\cssrgb\c80000\c80000\c80000;
\cssrgb\c33725\c61176\c83922;\cssrgb\c61176\c86275\c99608;\cssrgb\c83137\c83137\c83137;\cssrgb\c80784\c56863\c47059;\cssrgb\c86275\c86275\c66667;
}
\paperw11900\paperh16840\margl1440\margr1440\vieww11520\viewh8400\viewkind0
\deftab720
\pard\pardeftab720\partightenfactor0

\f0\fs24 \cf2 \cb3 \expnd0\expndtw0\kerning0
\outl0\strokewidth0 \strokec2 # If you come from bash you might have to change your $PATH.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # export PATH=$HOME/bin:/usr/local/bin:$PATH\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Path to your oh-my-zsh installation.\cf4 \cb1 \strokec4 \
\pard\pardeftab720\partightenfactor0
\cf5 \cb3 \strokec5 export\cf4 \strokec4  \cf6 \strokec6 ZSH\cf7 \strokec7 =\cf8 \strokec8 "\cf6 \strokec6 $HOME\cf8 \strokec8 /.oh-my-zsh"\cf4 \cb1 \strokec4 \
\
\pard\pardeftab720\partightenfactor0
\cf2 \cb3 \strokec2 # Set name of the theme to load --- if set to "random", it will\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # load a random theme each time oh-my-zsh is loaded, in which case,\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # to know which specific one was loaded, run: echo $RANDOM_THEME\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # See https://github.com/ohmyzsh/ohmyzsh/wiki/Themes\cf4 \cb1 \strokec4 \
\pard\pardeftab720\partightenfactor0
\cf6 \cb3 \strokec6 ZSH_THEME\cf7 \strokec7 =\cf8 \strokec8 "robbyrussell"\cf4 \cb1 \strokec4 \
\
\pard\pardeftab720\partightenfactor0
\cf2 \cb3 \strokec2 # Set list of themes to pick from when loading at random\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # Setting this variable when ZSH_THEME=random will cause zsh to load\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # a theme from this variable instead of looking in $ZSH/themes/\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # If set to an empty array, this variable will have no effect.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # ZSH_THEME_RANDOM_CANDIDATES=( "robbyrussell" "agnoster" )\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line to use case-sensitive completion.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # CASE_SENSITIVE="true"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line to use hyphen-insensitive completion.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # Case-sensitive completion must be off. _ and - will be interchangeable.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # HYPHEN_INSENSITIVE="true"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment one of the following lines to change the auto-update behavior\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # zstyle ':omz:update' mode disabled  # disable automatic updates\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # zstyle ':omz:update' mode auto      # update automatically without asking\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # zstyle ':omz:update' mode reminder  # just remind me to update when it's time\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line to change how often to auto-update (in days).\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # zstyle ':omz:update' frequency 13\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line if pasting URLs and other text is messed up.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # DISABLE_MAGIC_FUNCTIONS="true"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line to disable colors in ls.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # DISABLE_LS_COLORS="true"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line to disable auto-setting terminal title.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # DISABLE_AUTO_TITLE="true"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line to enable command auto-correction.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # ENABLE_CORRECTION="true"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line to display red dots whilst waiting for completion.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # You can also set it to another string to have that shown instead of the default red dots.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # e.g. COMPLETION_WAITING_DOTS="%F\{yellow\}waiting...%f"\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # Caution: this setting can cause issues with multiline prompts in zsh < 5.7.1 (see #5765)\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # COMPLETION_WAITING_DOTS="true"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line if you want to disable marking untracked files\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # under VCS as dirty. This makes repository status check for large repositories\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # much, much faster.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # DISABLE_UNTRACKED_FILES_DIRTY="true"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Uncomment the following line if you want to change the command execution time\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # stamp shown in the history command output.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # You can set one of the optional three formats:\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # "mm/dd/yyyy"|"dd.mm.yyyy"|"yyyy-mm-dd"\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # or set a custom format using the strftime function format specifications,\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # see 'man strftime' for details.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # HIST_STAMPS="mm/dd/yyyy"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Would you like to use another custom folder than $ZSH/custom?\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # ZSH_CUSTOM=/path/to/new-custom-folder\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Which plugins would you like to load?\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # Standard plugins can be found in $ZSH/plugins/\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # Custom plugins may be added to $ZSH_CUSTOM/plugins/\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # Example format: plugins=(rails git textmate ruby lighthouse)\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # Add wisely, as too many plugins slow down shell startup.\cf4 \cb1 \strokec4 \
\pard\pardeftab720\partightenfactor0
\cf6 \cb3 \strokec6 plugins\cf7 \strokec7 =\cf4 \strokec4 (\cf8 \strokec8 git\cf4 \strokec4 )\cb1 \
\
\cf6 \cb3 \strokec6 HOMEBREW_PREFIX\cf7 \strokec7 =\cf8 \strokec8 $(\cf9 \strokec9 brew\cf8 \strokec8  \cf5 \strokec5 --prefix\cf8 \strokec8 )\cf4 \cb1 \strokec4 \
\pard\pardeftab720\partightenfactor0
\cf5 \cb3 \strokec5 export\cf4 \strokec4  \cf6 \strokec6 FPATH\cf7 \strokec7 =\cf8 \strokec8 "$\{\cf6 \strokec6 HOMEBREW_PREFIX\cf8 \strokec8 \}/share/zsh/site-functions:$\{\cf6 \strokec6 FPATH\cf8 \strokec8 \}"\cf4 \cb1 \strokec4 \
\
\pard\pardeftab720\partightenfactor0
\cf9 \cb3 \strokec9 source\cf4 \strokec4  \cf6 \strokec6 $ZSH\cf8 \strokec8 /oh-my-zsh.sh\cf4 \cb1 \strokec4 \
\
\pard\pardeftab720\partightenfactor0
\cf2 \cb3 \strokec2 # User configuration\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # export MANPATH="/usr/local/man:$MANPATH"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # You may need to manually set your language environment\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # export LANG=en_US.UTF-8\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Preferred editor for local and remote sessions\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # if [[ -n $SSH_CONNECTION ]]; then\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 #   export EDITOR='vim'\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # else\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 #   export EDITOR='mvim'\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # fi\cf4 \cb1 \strokec4 \
\
\pard\pardeftab720\partightenfactor0
\cf5 \cb3 \strokec5 export\cf4 \strokec4  \cf6 \strokec6 EDITOR\cf7 \strokec7 =\cf8 \strokec8 "code -w"\cf4 \cb1 \strokec4 \
\
\pard\pardeftab720\partightenfactor0
\cf2 \cb3 \strokec2 # Compilation flags\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # export ARCHFLAGS="-arch x86_64"\cf4 \cb1 \strokec4 \
\
\cf2 \cb3 \strokec2 # Set personal aliases, overriding those provided by oh-my-zsh libs,\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # plugins, and themes. Aliases can be placed here, though oh-my-zsh\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # users are encouraged to define aliases within the ZSH_CUSTOM folder.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # For a full list of active aliases, run `alias`.\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 #\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # Example aliases\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # alias zshconfig="mate ~/.zshrc"\cf4 \cb1 \strokec4 \
\cf2 \cb3 \strokec2 # alias ohmyzsh="mate ~/.oh-my-zsh"\cf4 \cb1 \strokec4 \
\pard\pardeftab720\partightenfactor0
\cf5 \cb3 \strokec5 export\cf4 \strokec4  \cf6 \strokec6 NODE_AUTH_TOKEN\cf7 \strokec7 =\cf8 \strokec8 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\cf4 \cb1 \strokec4 \
\
\pard\pardeftab720\partightenfactor0
\cf9 \cb3 \strokec9 eval\cf4 \strokec4  \cf8 \strokec8 "$(\cf9 \strokec9 /opt/homebrew/bin/brew\cf8 \strokec8  shellenv)"\cf4 \cb1 \strokec4 \
\
\cf9 \cb3 \strokec9 eval\cf4 \strokec4  \cf8 \strokec8 "$(\cf9 \strokec9 starship\cf8 \strokec8  init zsh)"\cf4 \cb1 \strokec4 \
\
\pard\pardeftab720\partightenfactor0
\cf5 \cb3 \strokec5 alias\cf4 \strokec4  \cf6 \strokec6 python\cf7 \strokec7 =\cf4 \strokec4 /usr/bin/python3\cb1 \
\cf5 \cb3 \strokec5 export\cf4 \strokec4  \cf6 \strokec6 PATH\cf7 \strokec7 =\cf8 \strokec8 "/opt/homebrew/opt/libpq/bin:\cf6 \strokec6 $PATH\cf8 \strokec8 "\cf4 \cb1 \strokec4 \
\
}