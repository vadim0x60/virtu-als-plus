# Virtu-ALS+: Acute Care Simulator with a Programmatic Interface

This is the open source Unity project behind the [Virtu-ALS](https://apps.microsoft.com/store/detail/virtuals/9NVC18RLP321?hl=en-au&gl=AU) actute care simulator and [Auto-ALS](https://github.com/vadim0x60/auto-als) reinforcement learning environment. 
Most of the work was done by [Rob Brisk](https://www.linkedin.com/in/rob-brisk/) with some additions by [Vadim Liventsev](https://vadim.me) to make it playable programmatically.

## Reinforcement Learning Usage

You're in the wrong repository, you need [auto-als](https://github.com/vadim0x60/auto-als)

## Educational/Recreational Usage

1. Install Virtu-ALS [from the Microsoft Store](https://apps.microsoft.com/store/detail/virtuals/9NVC18RLP321?hl=en-au&gl=AU) or from the releases section of this repository.
2. If you're not a healthcare professional, take a look at [the ABCDE guidelines](https://www.resus.org.uk/library/abcde-approach) to understand what's expected of you
3. To quit the app, either hold ALT + F4 or press ESC and choose “Quit”. (You cannot currently
switch between modes - you have to close the app and re-open it to try a new scenario.)
4. The app in its current form is not a finished product, but rather a demonstration of some of the
features of which it is capable - please don’t be put off by the lack of polish.
6. It is worth trying out the scenarios from top to bottom - i.e. the arrested patient first, the
conscious patient second and the unconscious patient last.
7. The conscious and unconscious patients will have some form of unstable arrhythmia. To
complete the scenario, it will be necessary to either cardiovert or pace them (I’m not sure that
this is immediately obvious). This can be achieved by interacting directly with the buttons on
the defib.
8. The patients will arrest very quickly if their airway is obstructed, slightly less quickly if they are
hypoventilating and slowly if they are hypotensive with no fluids running. If your patients seem
to arrest almost immediately every time you use the app, try sorting out the airway as soon as
the scenario begins. (Don’t forget to re-assess after suctioning, as they might still be snoring.)
9. The app is cross-platform (OSX, Windows, iOS and Android), but we might have forgot to compile it for your platform. You can compile it yourself with [Unity](https://unity.com/) or open an issue that your platform is not supported and we'll build it for you.