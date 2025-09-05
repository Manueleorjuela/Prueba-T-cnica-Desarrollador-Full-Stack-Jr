import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Usersnew } from './usersnew';

describe('Usersnew', () => {
  let component: Usersnew;
  let fixture: ComponentFixture<Usersnew>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Usersnew]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Usersnew);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
