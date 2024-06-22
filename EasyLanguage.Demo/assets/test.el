#! /usr/bin/env elang
(program
 (define $sum (add 11 (add 22 33 44)))
 ([. Console WriteLine] $sum)
 ([. Console WriteLine] '"hello")
 )
