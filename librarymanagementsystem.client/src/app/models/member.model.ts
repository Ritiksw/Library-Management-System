export interface Member {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  membershipDate: string;
  isActive: boolean;
  activeLoans: number;
}

export interface CreateMember {
  fullName: string;
  email: string;
  phone: string;
}
