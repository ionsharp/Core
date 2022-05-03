using Imagin.Common.Collections.Generic;
using System;
using System.Management;

namespace Imagin.Common.Storage
{
    public class RemovableDriveEventArgs : EventArgs
    {
        public readonly StringDictionary Properties;

        public RemovableDriveEventArgs(StringDictionary properties)
        {
            Properties = properties;
        }
    }

    public delegate void RemovableDriveEventHandler(RemovableDriveEventArgs e);

    public class RemovableDrive
    {
        public static event RemovableDriveEventHandler Inserted;

        public static event RemovableDriveEventHandler Removed;

        static void OnInserted(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];

            var result = new StringDictionary();
            foreach (var property in instance.Properties)
                result.Add(property.Name, $"{property.Value}");

            Inserted?.Invoke(new RemovableDriveEventArgs(result));
        }

        static void OnRemoved(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];

            var result = new StringDictionary();
            foreach (var property in instance.Properties)
                result.Add(property.Name, $"{property.Value}");

            Removed?.Invoke(new RemovableDriveEventArgs(result));
        }

        static readonly WqlEventQuery insertQuery = new("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

        static readonly WqlEventQuery removeQuery = new("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");

        static ManagementEventWatcher insert;
        static ManagementEventWatcher Insert
        {
            get
            {
                if (insert == null)
                {
                    insert = new ManagementEventWatcher(insertQuery);
                    insert.EventArrived += OnInserted;
                    insert.Start();
                }
                return insert;
            }
        }

        static ManagementEventWatcher remove;
        static ManagementEventWatcher Remove
        {
            get
            {
                if (remove == null)
                {
                    remove = new ManagementEventWatcher(removeQuery);
                    remove.EventArrived += OnRemoved;
                    remove.Start();
                }
                return remove;
            }
        }
    }
}