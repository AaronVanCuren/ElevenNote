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
            var entity = _db.Notes.Single(e => e.NoteId == id && e.OwnerId == _userId);
            return
                new NoteDetail
                {
                    NoteId = entity.NoteId,
                    Title = entity.Title,
                    Content = entity.Content,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
        }

        public bool UpdateNote(NoteEdit model)
        {
            var entity = _db.Notes.Single(e => e.NoteId == model.NoteId && e.OwnerId == _userId);

            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.ModifiedUtc = DateTimeOffset.UtcNow;

            return _db.SaveChanges() == 1;
        }

        public bool DeleteNote(int noteId)
        {
            var entity = _db.Notes.Single(e => e.NoteId == noteId && e.OwnerId == _userId);
            _db.Notes.Remove(entity);
            return _db.SaveChanges() == 1;
        }
    }
}