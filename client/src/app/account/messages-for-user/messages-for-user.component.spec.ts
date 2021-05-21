import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessagesForUserComponent } from './messages-for-user.component';

describe('MessagesForUserComponent', () => {
  let component: MessagesForUserComponent;
  let fixture: ComponentFixture<MessagesForUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MessagesForUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MessagesForUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
