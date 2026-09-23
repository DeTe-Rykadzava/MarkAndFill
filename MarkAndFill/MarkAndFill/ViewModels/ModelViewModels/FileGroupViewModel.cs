using MarkAndFill.Base.Services;
using MarkAndFill.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkAndFill.ViewModels.ModelViewModels
{
    public class FileGroupViewModel
    {
        private readonly FileTempGroup _group;

        public string GroupName { get => _group.GroupName; }

        public FileGroupViewModel(FileTempGroup group)
        {
            _group = group;
        }

        public static List<FileGroupViewModel> GetGroups()
        {
            var groups = FileGroupService.GetAllGroups();
            return groups.Select(s => new FileGroupViewModel(s)).ToList();
        }

        public static async Task<FileGroupViewModel> CreateNewGroup(string groupName)
        {
            var group = new FileTempGroup(groupName);

            FileGroupService.NewGroup(group);
            await FileGroupService.SaveAsync();

            return new FileGroupViewModel(group);
        }

        public async Task RemoveGroup()
        {
            FileGroupService.RemoveGroup(_group);
            await FileGroupService.SaveAsync();
        }
    }
}
