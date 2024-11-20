import PageParams from './pageParams'

export default class PagedList<T> {
  items: [T]
  params: PageParams

  constructor(items: [T], params: PageParams) {
    this.items = items
    this.params = params
  }
}
