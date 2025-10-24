using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using Microsoft.Win32;      


namespace PSHOP
{
    class SerialLicenceKey
    {
        Int64 _SerialNo = 0;
        string _SerialKey = null;
        string _MachineId = null;

        public string ReadRegistry(string KeyName)
        {
            // Opening the registry key
            RegistryKey rk = Registry.LocalMachine;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey("SOFTWARE\\ParsinaERP");
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.

                    return (string)sk1.GetValue(KeyName);
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message + "\n Reading registry " + KeyName);
                    return null;
                }
            }
        }

        public bool WriteRegistry(string KeyName, object Value)
        {
            try
            {
                // Setting
                RegistryKey rk = Registry.LocalMachine;
                // I have to use CreateSubKey 
                RegistryKey sk1 = rk.CreateSubKey("SOFTWARE\\ParsinaERP");
                // Save the value
                sk1.SetValue(KeyName, Value);

                return true;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message + "\n Writing registry " + KeyName);
                return false;
            }
        }

        public bool CheckLicenceKey(string InputLicenceKey, string MachinId)
        {
            _SerialNo = 0;
            _SerialKey = null;

            if (InputLicenceKey == null | InputLicenceKey.Length < 13)
                return false;
            else
            {
                _MachineId =
                    Convert.ToChar(Convert.ToInt32(InputLicenceKey.Substring(0, 1)) + 65).ToString() +
                    MachinId.Substring(1, 1) +
                    Convert.ToChar(Convert.ToInt32(InputLicenceKey.Substring(1, 1)) + 65).ToString() +
                    MachinId.Substring(3, 1) +
                    Convert.ToChar(Convert.ToInt32(InputLicenceKey.Substring(2, 1)) + 65).ToString() +
                    MachinId.Substring(5, 1) +
                    Convert.ToChar(Convert.ToInt32(InputLicenceKey.Substring(6, 1)) + 65).ToString() +
                    MachinId.Substring(7, 1) +
                    Convert.ToChar(Convert.ToInt32(InputLicenceKey.Substring(10, 1)) + 65).ToString() +
                    MachinId.Substring(9, 1) +
                    Convert.ToChar(Convert.ToInt32(InputLicenceKey.Substring(11, 1)) + 65).ToString() +
                    MachinId.Substring(11, 1) +
                    Convert.ToChar(Convert.ToInt32(InputLicenceKey.Substring(12)) + 65).ToString();
            }

            for (int i = 0; i < _MachineId.Length; i++)
            {
                _SerialNo += Convert.ToInt64(_MachineId[i]);
            }

            if (_SerialNo > 0)
            {
                _SerialNo = Math.Abs((_SerialNo * 298) - 762763);
            }

            _SerialKey = "000000" + _SerialNo.ToString();
            _SerialKey = _SerialKey.Substring(_SerialKey.Length - 6, 6);
            _SerialKey =
                (Convert.ToInt32(Convert.ToChar(_MachineId.Substring(0, 1))) - 65).ToString() +
                (Convert.ToInt32(Convert.ToChar(_MachineId.Substring(2, 1))) - 65).ToString() +
                (Convert.ToInt32(Convert.ToChar(_MachineId.Substring(4, 1))) - 65).ToString() +
                _SerialKey.Substring(0, 1) +
                _SerialKey.Substring(1, 1) +
                _SerialKey.Substring(2, 1) +
                (Convert.ToInt32(Convert.ToChar(_MachineId.Substring(6, 1))) - 65).ToString() +
                _SerialKey.Substring(3, 1) +
                _SerialKey.Substring(4, 1) +
                _SerialKey.Substring(5, 1) +
                (Convert.ToInt32(Convert.ToChar(_MachineId.Substring(8, 1))) - 65).ToString() +
                (Convert.ToInt32(Convert.ToChar(_MachineId.Substring(10, 1))) - 65).ToString() +
                (Convert.ToInt32(Convert.ToChar(_MachineId.Substring(12))) - 65).ToString();

            if (_SerialKey == InputLicenceKey)
                return true;
            else
                return false;
        }

        public string GenerateLicenceKey(string InputId)
        {
            _SerialNo = 0;
            _SerialKey = null;

            int j = 0;

            for (int i = 0; i < InputId.Length; i++)
            {
                _SerialNo += Convert.ToInt64(InputId[i]);
            }

            if (_SerialNo > 0)
            {
                _SerialNo = Math.Abs((_SerialNo * 298) - 762763);
            }

            _SerialKey = "000000" + _SerialNo.ToString();
            _SerialKey = _SerialKey.Substring(_SerialKey.Length - 6, 6);
            _SerialKey =
                (Convert.ToInt32(Convert.ToChar(InputId.Substring(0, 1))) - 65).ToString() +
                (Convert.ToInt32(Convert.ToChar(InputId.Substring(2, 1))) - 65).ToString() +
                (Convert.ToInt32(Convert.ToChar(InputId.Substring(4, 1))) - 65).ToString() +
                _SerialKey.Substring(0, 1) +
                _SerialKey.Substring(1, 1) +
                _SerialKey.Substring(2, 1) +
                (Convert.ToInt32(Convert.ToChar(InputId.Substring(6, 1))) - 65).ToString() +
                _SerialKey.Substring(3, 1) +
                _SerialKey.Substring(4, 1) +
                _SerialKey.Substring(5) +
                (Convert.ToInt32(Convert.ToChar(InputId.Substring(8, 1))) - 65).ToString() +
                (Convert.ToInt32(Convert.ToChar(InputId.Substring(10, 1))) - 65).ToString() +
                (Convert.ToInt32(Convert.ToChar(InputId.Substring(12))) - 65).ToString();

            return _SerialKey;
        }

        public string GetMachineId()
        {
            string _machineid = null, _cpuSerial = null;

            //_hardSerial = null, _hardName = null;
            //string _hardInfo = "SELECT * FROM Win32_DiskDrive";
            //string _hardId = "SELECT * FROM Win32_PhysicalMedia";

            string _cpuId = "SELECT * FROM Win32_Processor";
            string Part1 = null, Part2 = null;
            //, Part3 = null;

            ManagementObjectSearcher searcher;

            searcher = new ManagementObjectSearcher(_cpuId);
            foreach (ManagementObject wmi_CPU in searcher.Get())
                if (wmi_CPU["ProcessorId"] != null)
                    _cpuSerial += wmi_CPU.Properties["ProcessorId"].Value.ToString();

            //searcher = new ManagementObjectSearcher(_hardInfo);
            //foreach (ManagementObject wmi_HD in searcher.Get())
            //    if (wmi_HD["Model"] != null)
            //        _hardName += wmi_HD["Model"].ToString();

            //searcher = new ManagementObjectSearcher(_hardId);
            //foreach (ManagementObject wmi_HD in searcher.Get())
            //    if (wmi_HD["SerialNumber"] != null)
            //        _hardSerial += wmi_HD["SerialNumber"].ToString();

            //_hardName = _hardName.Replace(" ", "");

            Part1 = _cpuSerial.Substring(0, 1) +
                _cpuSerial.Substring(_cpuSerial.Length - 5, 5);

            Random _Random = new Random();

            Part2 = Math.Round(_Random.NextDouble() * 10000000).ToString();
            Part2 = "0000000" + Part2;
            Part2 = Part2.Substring(Part2.Length - 7, 7);

            //Part3 = _hardName.Substring(0, 5);
            //if (_hardSerial.Length >= 4)
            //    Part3 += _hardSerial.Substring(0, 4);

            _machineid =
                Convert.ToChar(Convert.ToInt32(Part2.Substring(0, 1)) + 65).ToString() +
                Part1.Substring(0, 1) +
                Convert.ToChar(Convert.ToInt32(Part2.Substring(1, 1)) + 65).ToString() +
                Part1.Substring(1, 1) +
                Convert.ToChar(Convert.ToInt32(Part2.Substring(2, 1)) + 65).ToString() +
                Part1.Substring(2, 1) +
                Convert.ToChar(Convert.ToInt32(Part2.Substring(3, 1)) + 65).ToString() +
                Part1.Substring(3, 1) +
                Convert.ToChar(Convert.ToInt32(Part2.Substring(4, 1)) + 65).ToString() +
                Part1.Substring(4, 1) +
                Convert.ToChar(Convert.ToInt32(Part2.Substring(5, 1)) + 65).ToString() +
                Part1.Substring(5) +
                Convert.ToChar(Convert.ToInt32(Part2.Substring(6)) + 65).ToString();


            return _machineid;
        }

    }

    



}
