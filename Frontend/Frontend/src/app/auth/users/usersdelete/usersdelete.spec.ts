import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Usersdelete } from './usersdelete';

describe('Usersdelete', () => {
  let component: Usersdelete;
  let fixture: ComponentFixture<Usersdelete>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Usersdelete]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Usersdelete);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
