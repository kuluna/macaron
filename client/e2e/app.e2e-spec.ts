import { MacaronPage } from './app.po';

describe('macaron App', () => {
  let page: MacaronPage;

  beforeEach(() => {
    page = new MacaronPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    // expect(page.getParagraphText()).toEqual('app works!');
  });
});
