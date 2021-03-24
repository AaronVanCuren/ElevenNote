using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        ApplicationDbContext _db = new ApplicationDbContext();

        public bool CreateNote(NoteCreate model)
        {
            var note =
                new Note()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now
                };

            _db.Notes.Add(note);
            return _db.SaveChanges() == 1;
        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            var query = _db.Notes.Where(e => e.OwnerId == _userId)
                    .Select(
                        e => new NoteListItem
                        {
                            NoteId = e.NoteId,
                            Title = e.Title,
                            CreatedUtc = e.CreatedUtc
                        });

            return query.ToArray();
        }

        public NoteDetail GetNoteById(int id)
        {
            var note = _db.Notes.Single(e => e.NoteId == id && e.OwnerId == _userId);
            return
                new NoteDetail
                {
                    NoteId = note.NoteId,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedUtc = note.CreatedUtc,
                    ModifiedUtc = note.ModifiedUtc
                };
        }

        public bool UpdateNote(NoteEdit model)
        {
            var note = _db.Notes.Single(e => e.NoteId == model.NoteId && e.OwnerId == _userId);

            note.Title = model.Title;
            note.Content = model.Content;
            note.ModifiedUtc = DateTimeOffset.UtcNow;

            return _db.SaveChanges() == 1;
        }

        public bool DeleteNote(int noteId)
        {
            var note = _db.Notes.Single(e => e.NoteId == noteId && e.OwnerId == _userId);
            _db.Notes.Remove(note);
            return _db.SaveChanges() == 1;
        }
    }
}