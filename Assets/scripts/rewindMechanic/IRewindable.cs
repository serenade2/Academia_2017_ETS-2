using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewindable {

    void Rewind(bool isRewinding);
    void Pause(bool isPaused);
    void FastForward(bool isFastForwarding);
}
