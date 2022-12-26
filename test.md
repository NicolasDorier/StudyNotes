A continuous function can be represented as a vector of arbitrary high dimension.
For example, let's take two arbitrary functions `f` and `g` in vector notation.

$$f = \begin{bmatrix}
..\\
0\\
1\\
4\\
16\\\
..
\end{bmatrix}$$

$$g= \begin{bmatrix}
..\\
2\\
3\\
1\\
6\\
..
\end{bmatrix}$$

It turns out that it simplifies the definition of the following integral by reducting it to a simple dot product.
$$\int f(x) * g(x) \,dx = f . g$$

This is also equal to `f` represented as a one row matrix multiplied by `g`.

$$\begin{bmatrix}
..& 0 & 1 & 4 & 16 & ..
\end{bmatrix}

\begin{bmatrix}
..\\
2\\
3\\
1\\
6\\
..
\end{bmatrix} =
\begin{bmatrix}
f.g
\end{bmatrix}
$$


Now, imagine that `f` get translated by a parameter `t` along `g`.
In a vector representation, you could extend the row matrix `f` by additional rows, each rows representing `f` being shifted by some amount.

$$\begin{bmatrix}
..\\
..& 0 & 1 & 4 & 16 & ..\\
..& 1 & 4 & 16 & 33 & ..\\
..& 4 & 16 & 33 & 57 & ..\\
..
\end{bmatrix}
\begin{bmatrix}
..\\
2\\
3\\
1\\
6\\
..
\end{bmatrix}=
\begin{bmatrix}
..\\
f.g\\
32\\
2\\
-24\\
..
\end{bmatrix}$$

The resulting vector is the vector representation of the correlation of `f` and `g`, which is normally written like that:
$$(f*g)(t)=\int f(x+t) * g(x) \,dx$$

The interesting point is that the resulting vector can be interpreted as a measure of similarity between the two functions for each translation of `f`.

This is because each value in the resulting vector is a dot product between `g` and the translated version of `f`.


$$ f.g = |f| * |g| * cos(fg)$$
Since `|f| * |g|` is constant for all the versions of translated `f`, the only thing changing is the `cos(fg)` varying from -1 to 1.

So the highest the value is, the more the translated version of `f` looks like `g`.

## How does this work with complex numbers?

Same process, but we have to take the subjugate of the function.
The resulting vector is an imaginary number.

So similarity you need to take the magnitude of it.

A peak in magnitude means the signals are "aligned" although they may be out of phase. The phase tells you how many degrees out of phase the two signals are.

