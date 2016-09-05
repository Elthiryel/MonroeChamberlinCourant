%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% 
% File:     aghdpl.cls           (LaTeX Document class option "aghdpl")
%          
% Author:  Marcin Szpyrka     (mszpyrka@agh.edu.pl)
%          Grzegorz J. Nalepa (gjn@agh.edu.pl)
%          AGH University of Science and Technology, Kraków, POLAND
% 
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

\message{*** [aghdpl.cls] ---  (MSz, GJN)  v2.0 <19.11.2013>  ***}       

\newif\ifpdflatex\pdflatexfalse
\NeedsTeXFormat{LaTeX2e} % 
\ProvidesClass{aghdpl} 
\DeclareOption{pdflatex}{\pdflatextrue}
\DeclareOption*{\PassOptionsToClass{\CurrentOption}{report}} 
\ProcessOptions\LoadClass[a4paper,twoside,openright]{report} 

\RequirePackage{times}
\RequirePackage[T1]{fontenc}
\RequirePackage{graphicx}
\RequirePackage{geometry}
\RequirePackage[usenames]{color}
\RequirePackage{fancyhdr}
\RequirePackage{url}
\RequirePackage{titlesec}
\RequirePackage{tocloft}

\geometry{lmargin=30mm, rmargin=20mm, tmargin=30mm, bmargin=30mm, foot=10mm, head=10mm}
\definecolor{Green}{rgb}{0.1,0.5,0.1}

\def\shorttitlePL#1   {\gdef\@shorttitlePL{#1}}
\def\shorttitleEN#1   {\gdef\@shorttitleEN{#1}}
\def\shortauthor#1    {\gdef\@shortauthor{#1}}
\def\titlePL#1        {\gdef\@titlePL{#1}}
\def\titleEN#1        {\gdef\@titleEN{#1}}
\def\faculty#1      {\gdef\@faculty{#1}}
\def\thesistype#1   {\gdef\@thesistype{#1}}
\def\supervisor#1   {\gdef\@supervisor{#1}}
\def\department#1   {\gdef\@department{#1}}
\def\degreeprogramme#1 {\gdef\@degreeprogramme{#1}}


%\bibliographystyle{abbrv}
\linespread{1.3}
\renewcommand{\labelitemi}{--}

%------------------------------------------------------------------------------


\def\thesisheaders{
\fancyfoot[LE,RO]{\small \@shortauthor\quad\textit{\@shorttitlePL}}
}

\def\thesistable{
\begin{tabular}{p{45mm}l}
Autor: & {\itshape \@author}\\[-1mm]
Kierunek studiów: & {\itshape \@degreeprogramme}\\[-1mm]
Opiekun pracy: & {\itshape \@supervisor}\\
\end{tabular}
}

\DeclareOption{en}{
\def\thesisheaders{
\fancyfoot[LE,RO]{\small \@shortauthor\quad\textit{\@shorttitleEN}}
}

\def\thesistable{
\begin{tabular}{p{45mm}l}
Autor: & {\itshape \@author}\\[-1mm]
Kierunek studiów: & {\itshape \@degreeprogramme}\\[-1mm]
Opiekun pracy: & {\itshape \@supervisor}\\
\end{tabular}
}
}

\ProcessOptions

\newcommand{\titlepages}{%

\newpage \titlepage 

\begin{center}
\vspace*{1.5cm}

\includegraphics[scale=0.6]{agh}

\vspace*{2mm}

{\sffamily{\large{\bfseries{\scshape Akademia G\'{o}rniczo-Hutnicza im. Stanis\l{}awa Staszica w Krakowie}}}

\vspace*{3mm}

{\small\bfseries \MakeUppercase{\@faculty}}}
\vspace*{3mm}

{\small{\MakeUppercase{\@department}}}
\vspace*{12mm}

{\sffamily{\small\bfseries \MakeUppercase{\@thesistype}}}
\vspace*{12mm}

{\sffamily\Large\itshape \@titleEN}
\vspace*{3mm}

{\sffamily\small\itshape \@titlePL}
\vspace*{38mm}
\end{center}

\noindent
{\sffamily
\thesistable
}

\vspace*{10mm}

\centerline{\small\sffamily Krak\'{o}w, \@date}

\clearpage \titlepage 

{~~}\vspace{20mm}

%{\large\itshape O\'swiadczam, \'swiadomy(-a) odpowiedzialno\'sci karnejœ za po\'swiadczenie nieprawdy, \.ze \mbox{niniejsz\k{a}} prac\k{e} dyplomow\k{a} wykona\l{}em(-am) osobi\'scie i samodzielnie i nie korzysta\l{}em(-am) ze \'zr\'ode\l{} innych ni\.z wymienione w pracy.}
{\large\itshape Uprzedzony o odpowiedzialno\'sci karnej na podstawie art. 115 ust. 1 i 2 ustawy z dnia 4 lutego 1994 r. o prawie autorskim i prawach pokrewnych (t.j. Dz.U. z 2006 r. Nr 90, poz. 631 z p\'o\'zn. zm.): ,,Kto przyw\l{}aszcza sobie autorstwo albo wprowadza w b\l{}\k{a}d co do autorstwa ca\l{}o\'sci lub cz\k{e}\'sci cudzego utworu albo artystycznego wykonania, podlega grzywnie, karze ograniczenia wolno\'sci albo pozbawienia wolno\'sci do lat 3. Tej samej karze podlega, kto rozpowszechnia bez podania nazwiska lub pseudonimu tw\'orcy cudzy utw\'or w wersji oryginalnej albo w postaci opracowania, artystyczne wykonanie, fonogram, wideogram lub nadanie.", a tak\.ze uprzedzony o odpowiedzialno\'sci dyscyplinarnej na podstawie art. 211 ust. 1 ustawy z dnia 27 lipca 2005 r. Prawo o szkolnictwie wy\.zszym (t.j. Dz. U. z 2012 r. poz. 572, z p\'o\'zn. zm.) ,,Za naruszenie przepis\'ow obowi\k{a}zuj\k{a}cych w uczelni oraz za czyny uchybiaj\k{a}ce godno\'sci studenta student ponosi odpowiedzialno\'s\'c dyscyplinarn\k{a} albo przed s\k{a}dem kole\.zenskim samorz\k{a}du studenckiego, zwanym dalej ,,s\k{a}dem kole\.ze\'nskim", o\'swiadczam, \.ze niniejsz\k{a} prac\k{e} dyplomow\k{a} wykona\l{}em osobi\'scie i samodzielnie i nie korzysta\l{}em ze \'zr\'ode\l{} innych ni\.z wymienione w pracy. }

\clearpage

\setcounter{page}{3}

}


\pagestyle{fancy}
\renewcommand{\chaptermark}[1]{\markboth{\thechapter.\ #1}{}}
\renewcommand{\sectionmark}[1]{\markright{\thesection.\ #1}}
\renewcommand{\headrulewidth}{0.5pt}
\renewcommand{\footrulewidth}{0.5pt}
\fancyhf{}
\fancyhead{}
%\fancyhead[L]{\slshape{\small \rightmark}}
\fancyhead[RE,LO]{\slshape{\small \rightmark}}
%\fancyhead[R]{\bfseries \thepage}
\fancyhead[RO,LE]{\bfseries \thepage}

\thesisheaders

\frenchspacing
\sloppy 
\widowpenalty     9999 
\clubpenalty      9999 
\flushbottom

%---------------------------------------------------------------------------
% Ustawienia parametrów dla tytu³ów rozdzia³ów, podrozdzia³ów, itd.
%---------------------------------------------------------------------------


\titleformat{\chapter}[block]
{\bfseries\LARGE}{\filright \LARGE\thechapter. }{0ex}{}
%odstêpy: lewy, góra, dó³
\titlespacing{\chapter}{0pt}{17mm}{19mm}
\titleformat{\section}[block]
{\bfseries\Large}{\filright \Large\thesection. }{0ex}{}
\titlespacing{\section}{7mm}{8mm plus 0mm minus 1mm}{4mm plus 0mm minus 1mm}
\titleformat{\subsection}[block]
{\bfseries\large}{\filright \large\thesubsection. }{0ex}{}
\titlespacing{\subsection}{7mm}{6mm}{2.5mm}
\titleformat{\subsubsection}[block]
{\bfseries\normalsize}{\filright \normalsize\thesubsubsection. }{0ex}{}
\titlespacing{\subsubsection}{7mm}{3mm}{1.5mm}

%---------------------------------------------------------------------------
% Ustawienia parametrów generowania spisu tre¶ci i spisu rysunków
%---------------------------------------------------------------------------

\renewcommand{\cfttoctitlefont}{\bfseries\LARGE}
\renewcommand{\cftloftitlefont}{\bfseries\LARGE}
\renewcommand{\cftlottitlefont}{\bfseries\LARGE}
\renewcommand{\cftbeforetoctitleskip}{20mm}
\renewcommand{\cftaftertoctitleskip}{19mm}
\renewcommand{\cftchapleader}{\cftdotfill{\cftdot}}
\renewcommand{\cftsecleader}{\cftdotfill{\cftdot}}
\renewcommand{\cftsubsecleader}{\cftdotfill{\cftdot}}
\renewcommand{\cftsubsubsecleader}{\cftdotfill{\cftdot}}
\renewcommand{\cftchapaftersnum}{.}
\renewcommand{\cftsecaftersnum}{.}
\renewcommand{\cftsubsecaftersnum}{.}
\setlength{\cftparskip}{2pt}
\setlength{\cftbeforechapskip}{0mm}
\renewcommand{\cftchappagefont}{\normalsize}
