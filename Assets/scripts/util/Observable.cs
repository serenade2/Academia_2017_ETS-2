using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomUtile
{
    public interface Observable
    {
        void addObserver(Observer o);
        void setChanged();
        void notify();
    }  
}

