using ElevenNote.Data;
using ElevenNote.Models;
using ElevenNote.WebMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
	public class NoteService
	{
		// private field
		private readonly Guid _userId;

		//constructor
		public NoteService(Guid userId) 
		{
			_userId = userId;
		}

		// Create note
		public bool CreateNote(NoteCreate model) 
		{
			var entity =
				new Note()
				{
					OwnerId = _userId,
					Title = model.Title,
					Content = model.Content,
					CreatedUtc = DateTimeOffset.Now
				};

			using (var ctx = new ApplicationDbContext()) 
			{
				ctx.Notes.Add(entity);
				return ctx.SaveChanges() == 1;
			}
		}

		// Get notes from a specific user
		public IEnumerable<NoteListItem> GetNotes() 
		{
			using (var ctx = new ApplicationDbContext()) 
			{
				var query =
					ctx
						.Notes
						.Where(e => e.OwnerId == _userId)
						.Select(
							e =>
								new NoteListItem
								{
									NoteId = e.NoteId,
									Title = e.Title,
									CreatedUtc = e.CreatedUtc
								}
						);
				return query.ToArray();
			}
		}
	}
}
