using System;
using System.Collections;
using System.Collections.Generic;

public struct pBitWiseButtons
{
    private int _buttonsState;

    public int ButtonState {  get { return _buttonsState; } }

    public void Set(int btnID, bool state)
    {
        if(state)
            SetDown(btnID);
        else
            SetUp(btnID);
    }

    public void SetDown(int btnID)
    {
        _buttonsState |= (1 << btnID); 
    }

    public void SetUp(int btnID)
    {
        _buttonsState &= ~(1 << btnID);
    }
    public bool IsButtonSetDown(int btnID)
    {
        return (_buttonsState & (1 << btnID)) != 0;
    }

    public pBitWiseButtons GetPressed(pBitWiseButtons previous)
    {
        previous._buttonsState = (previous._buttonsState ^ _buttonsState) & _buttonsState;
        return previous;
    }

    public pBitWiseButtons GetReleased(pBitWiseButtons previous)
    {
        previous._buttonsState = (previous._buttonsState ^ _buttonsState) & previous._buttonsState;
        return previous;
    }

}
