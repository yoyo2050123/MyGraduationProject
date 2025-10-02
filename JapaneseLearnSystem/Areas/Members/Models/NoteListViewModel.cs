using JapaneseLearnSystem.Models;

namespace JapaneseLearnSystem.Areas.Members.Models
{
    public class NoteListViewModel
    {
        public List<NoteViewModel> Notes { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
