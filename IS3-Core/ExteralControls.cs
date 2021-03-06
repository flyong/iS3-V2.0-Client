﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace iS3.Core
{
    #region Copyright Notice
    //************************  Notice  **********************************
    //** This file is part of iS3
    //**
    //** Copyright (c) 2015 Tongji University iS3 Team. All rights reserved.
    //**
    //** This library is free software; you can redistribute it and/or
    //** modify it under the terms of the GNU Lesser General Public
    //** License as published by the Free Software Foundation; either
    //** version 3 of the License, or (at your option) any later version.
    //**
    //** This library is distributed in the hope that it will be useful,
    //** but WITHOUT ANY WARRANTY; without even the implied warranty of
    //** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    //** Lesser General Public License for more details.
    //**
    //** In addition, as a special exception,  that plugins developed for iS3,
    //** are allowed to remain closed sourced and can be distributed under any license .
    //** These rights are included in the file LGPL_EXCEPTION.txt in this package.
    //**
    //**************************************************************************
    #endregion

    // Summary:
    //     This class is intended for defining user-defined control tree items.
    //     Supply a display name and a call back function to define a control tree item.
    // Remarks:
    //     ControlTreeItem example:
    //         displayPath: "Charts"
    //         displayName: "Pie Chart"
    //         func: void test();
    //
    public class ControlTreeItem
    {
        // Summary:
        //     Display path
        public string displayPath { get; set; }

        // Summary:
        //     Display name
        public string displayName { get; set; }

        // Summary:
        //     Child items
        public ObservableCollection<ControlTreeItem> items { get; set; }


        public IBaseView view { get; set; }
        //// Summary:
        ////     Call back function
        //public delegate void DelegateFunc();
        //public DelegateFunc func;

        public ControlTreeItem(string path, string name)
        {
            displayPath = path;
            displayName = name;
            items = new ObservableCollection<ControlTreeItem>();
        }
        public ControlTreeItem(string path, string name, IBaseView view)
        {
            displayPath = path;
            displayName = name;
            this.view = view;
        }

        // Summary:
        //     Find the ToolTree by a path and name
        // Remarks:
        //     A examples of pathName:
        //       "Toolsboxes|Geology|ToolName"
        public ControlTreeItem find(string pathName)
        {
            string[] names = pathName.Split(new char[] { '\\', '/', '|' });
            ControlTreeItem tree = this;
            string name = null;
            for (int i = 0; i < names.Count(); ++i)
            {
                name = names[i];
                tree = tree.items.FirstOrDefault(x => x.displayName == name);
                if (tree == null)
                    return null;
            }
            return tree;
        }

        // Summary:
        //     Add the ControlTreeItem
        // Parameter:
        //     path: path to the ControlTreeItem
        //     A examples of path:
        //       "Charts"
        public void add(ControlTreeItem item)
        {
            string path = item.displayPath;
            if (path == null || path.Length == 0)
            {
                this.items.Add(item);
                return;
            }

            string[] names = path.Split(new char[] { '\\', '/', '|' });
            ControlTreeItem tree = this;
            string name = null;
            for (int i = 0; i < names.Count(); ++i)
            {
                name = names[i];
                if (name.Length == 0)
                    continue;
                ControlTreeItem child = tree.items.FirstOrDefault(x => x.displayName == name);
                if (child == null)
                {
                    child = new ControlTreeItem(null, name);
                    tree.add(child);
                }
                tree = child;
            }
            tree.items.Add(item);
        }
    }

    // Summary:
    //     This class is intended for defining user-defined UI Controls.
  
    public class ExteralControls : Extensions
    {
        // Summary:
        //     Name, version and provide of the tool
        public override string name() { return "Unknown UI Controls"; }

        // Summary:
        //     Get treeItems of the tool, called immediately after loaded.
        public virtual IEnumerable<ControlTreeItem> treeItems()
        {
            return null;
        }
    }
}
