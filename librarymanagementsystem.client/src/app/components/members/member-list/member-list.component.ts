import { Component, OnInit } from '@angular/core';
import { MemberService } from '../../../services/member.service';
import { Member } from '../../../models/member.model';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];
  search = '';

  constructor(private memberService: MemberService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.memberService.getAll(this.search || undefined).subscribe(data => this.members = data);
  }

  onSearch(): void {
    this.load();
  }

  deleteMember(id: number): void {
    if (confirm('Are you sure you want to delete this member?')) {
      this.memberService.delete(id).subscribe({
        next: () => this.load(),
        error: (err) => alert(err.error?.message || 'Cannot delete this member.')
      });
    }
  }
}
