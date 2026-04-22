import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberService } from '../../../services/member.service';
import { CreateMember } from '../../../models/member.model';

@Component({
  selector: 'app-member-form',
  templateUrl: './member-form.component.html',
  styleUrls: ['./member-form.component.css']
})
export class MemberFormComponent implements OnInit {
  isEdit = false;
  memberId = 0;
  errorMessage = '';

  model: CreateMember = {
    fullName: '',
    email: '',
    phone: ''
  };

  constructor(
    private memberService: MemberService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.memberId = +id;
      this.memberService.getById(this.memberId).subscribe(member => {
        this.model = {
          fullName: member.fullName,
          email: member.email,
          phone: member.phone
        };
      });
    }
  }

  save(): void {
    this.errorMessage = '';
    const action: Observable<any> = this.isEdit
      ? this.memberService.update(this.memberId, this.model)
      : this.memberService.create(this.model);

    action.subscribe({
      next: () => this.router.navigate(['/members']),
      error: (err: any) => this.errorMessage = err.error?.message || 'An error occurred.'
    });
  }
}
