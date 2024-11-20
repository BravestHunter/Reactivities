import { ProfileShort } from '../profileShort'

export default interface ActivityDto {
  id: number
  title: string
  date: Date
  description?: string
  category: string
  city: string
  venue: string
  isCancelled: boolean
  host: ProfileShort
  attendees: ProfileShort[]
}
