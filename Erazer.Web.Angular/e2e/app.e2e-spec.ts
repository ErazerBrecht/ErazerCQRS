import { CqrsFrontEndPage } from './app.po';

describe('cqrs-front-end App', () => {
  let page: CqrsFrontEndPage;

  beforeEach(() => {
    page = new CqrsFrontEndPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
