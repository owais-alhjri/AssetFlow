import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssetFormDialog } from './asset-form-dialog';

describe('AssetFormDialog', () => {
  let component: AssetFormDialog;
  let fixture: ComponentFixture<AssetFormDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssetFormDialog],
    }).compileComponents();

    fixture = TestBed.createComponent(AssetFormDialog);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
