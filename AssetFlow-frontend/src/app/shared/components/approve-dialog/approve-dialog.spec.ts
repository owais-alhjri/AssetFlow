import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApproveDialog } from './approve-dialog';

describe('ApproveDialog', () => {
  let component: ApproveDialog;
  let fixture: ComponentFixture<ApproveDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApproveDialog],
    }).compileComponents();

    fixture = TestBed.createComponent(ApproveDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
