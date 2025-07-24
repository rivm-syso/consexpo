# Patch

## Bugfix

We needed to apply a bugfix to DotNumerics, see [/third_party/patches/dotnumerics-stepsizefix.md](/third_party/patches/dotnumerics-stepsizefix.md).

It is recommended you apply this bugfix.

## Alternative

If for some reason, you are not able to patch DotNumerics, you can revert the bugfix in the ConsExpo code by applying this patch:
[patches/dotnumerics-revert-stepsizefix.diff](patches/dotnumerics-revert-stepsizefix.diff), using a Git client.

Note: reverting the bugfix will make ConsExpo behave differently than intended by the ConsExpo-team and yield different results than the online tool [ConsExpo Web](https://consexpoweb.nl/).