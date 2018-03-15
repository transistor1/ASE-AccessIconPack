using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccessIconPack.Models;
using MsaSQLEditor.Interfaces;

namespace AccessIconPack
{
    public class AccessIconPack : IPlugin2
    {
        public string Name => "Access Icon Pack";

        public void Initialize(IPluginContext2 context)
        {
            string localPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Field Effect, LLC", "MsaSQLEditor");

            if (Directory.Exists(localPath))
                return;

            var loadIconPack = new LoadIconPack(localPath);
            loadIconPack.Extract();
        }

        public void Unload()
        {
            
        }
    }
}
