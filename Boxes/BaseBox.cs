// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================
    
using UnityEngine;

namespace Chuozt.Template
{
    public class BaseBox : MonoBehaviour
    {
        public virtual void OnEnable()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        public virtual void OnOpen()
        {

        }

        public virtual void OnClose()
        {

        }
    }
}