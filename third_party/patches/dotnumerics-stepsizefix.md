# Patch

## Bugfix

We needed to patch the DotNumerics library, to resolve a bug that resulted in `STEP SIZE BECOMES TOO SMALL` exceptions.

When the `OdeSolver` approaches the end of the integration interval, it will reduce its step size to the end of the interval, when needed. If the precision of the step is too small, it will half the step size. Integration may proceed two steps, but due to rounding errors the end of the interval may not be reached exactly. The remaining interval is tested for its length and compared to the minimum step size, resulting in a `STEP SIZE BECOMES TOO SMALL` exception.

The bugfix will slightly reduce the integration interval when this exception is bound to occur.

## Source code

However, we are not licensed to distribute DotNumerics. To run ConsExpo code with the fix, you need to download the source code of DotNumerics yourself and apply the patch provided: [dotnumerics-stepsizefix.diff](dotnumerics-stepsizefix.diff).

The source code of DotNumerics can be obtained via the [DotNumerics website](http://dotnumerics.com/Downloads.aspx) or be found at [Internet archive, snapshot of http://dotnumerics.com/Downloads.aspx at 2017-02-22 05:17:58](https://web.archive.org/web/20170222051758/http://www.dotnumerics.com:80/Downloads.aspx).

## Applying the patch

### Project code

1. Unzip the DotNumerics source code in the directory `Source\DotNumerics` to a folder named `DotNumerics` in the directory [\src](\src).
1. Rename the project file `DotNumerics VS2008.csproj` to `DotNumerics VS2008.csproj` 
1. Start the VS.Net IDE and include the project file in the ConsExpo solution. VS.Net will upgrade the project file.

### Patch file

The patch is contained in the file [dotnumerics-stepsizefix.diff](dotnumerics-stepsizefix.diff). Use a git client to apply the patch.

### References

Finally, you need to changes the binary references in the projects `RIVM.ConsExpo.Model` and `RIVM.ConsExpo.Model.Tests` into projects references to the DotNumerics project you added.
Failing to follow these instructions, will result in 2 build errors.

## Alternative

Alternatively, you can revert our patch. For more information, see [/patches/dotnumerics-revert-stepsizefix.md](/patches/dotnumerics-revert-stepsizefix.md).
