// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================

namespace GameCore.DesignPatterns
{
    public interface ISubject<T>
    {
        void Subscribe(string eventName, IObserver<T> observer);
        void Unsubscribe(string eventName, IObserver<T> observer);
    }
}