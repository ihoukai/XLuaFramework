using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XLuaFramework;
using System.Reflection;
using UnityEngine.UI;

public class View : Base, IView {

    protected void Awake()
    {
        InitFieldsAttribute();
        InitMethodsAttribute();
    }

    public virtual void OnMessage(IMessage message) {
    }

    private void InitFieldsAttribute()
    {
        Type type = this.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        int len = fields.Length;
        for (int i = 0; i < len; i++)
        {
            object[] objs = fields[i].GetCustomAttributes(typeof(BindViewAttribute), false);
            if (objs.Length != 0)
            {
                BindViewAttribute attri = (BindViewAttribute)objs[0];
                Transform transform = this.gameObject.transform.Find(attri.View);
                if (transform == null)
                {
                    Debug.LogError(this.name + "类的BindView(\"" + attri.View + "\")没有匹配的GameObject");
                    continue;
                }
                Component componet = transform.GetComponent(fields[i].FieldType);
                if (componet == null)
                {
                    Debug.LogError(this.name + "类的BindView(\"" + attri.View + "\")没有匹配的" + fields[i].FieldType.ToString() + "组件");
                    continue;
                }
                fields[i].SetValue(this, componet);
            }
        }
    }

    private void InitMethodsAttribute()
    {
        Type type = this.GetType();
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        int len = methods.Length;
        for (int i = 0; i < len; i++)
        {
            MethodInfo methodinfo = methods[i];
            object[] objs = methodinfo.GetCustomAttributes(typeof(OnClickAttribute), false);
            if (objs.Length != 0)
            {
                OnClickAttribute attri = (OnClickAttribute)objs[0];
                Transform transform = this.gameObject.transform.Find(attri.View);
                if (transform == null)
                {
                    Debug.LogError(this.name + "类的OnClick(\"" + attri.View + "\")没有匹配的GameObject");
                    continue;
                }

                Button btn = transform.GetComponent<Button>();
                if (btn == null)
                {
                    btn = transform.gameObject.AddComponent<Button>();
                }

                btn.onClick.AddListener(delegate {
                    methodinfo.Invoke(this, null);
                });
            }
        }
    }
}
