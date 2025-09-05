import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Usersedit } from './usersedit';

describe('Usersedit', () => {
  let component: Usersedit;
  let fixture: ComponentFixture<Usersedit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Usersedit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Usersedit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
