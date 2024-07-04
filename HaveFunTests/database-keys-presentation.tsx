import React, { useState } from 'react';
import { ChevronLeft, ChevronRight, Globe } from 'lucide-react';

const translations = {
  en: {
    title: "Database Tables: Primary and Foreign Keys",
    primaryKey: "Primary Key",
    foreignKeys: "Foreign Keys",
    none: "None",
    slide: "Slide",
    of: "of",
    references: "references"
  },
  zh: {
    title: "資料庫表格：主鍵和外鍵",
    primaryKey: "主鍵",
    foreignKeys: "外鍵",
    none: "無",
    slide: "幻燈片",
    of: "共",
    references: "參考"
  }
};

const Slide = ({ title, content, lang }) => (
  <div className="bg-white rounded-lg shadow-lg p-6 m-4">
    <h2 className="text-2xl font-bold mb-4">{title}</h2>
    <div className="text-left">{content(lang)}</div>
  </div>
);

const Presentation = () => {
  const [currentSlide, setCurrentSlide] = useState(0);
  const [lang, setLang] = useState('en');

  const toggleLang = () => {
    setLang(lang === 'en' ? 'zh' : 'en');
  };

  const t = translations[lang];

  const slides = [
    {
      title: "Activities",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>UserId ({t.references} UserInfos)</li>
            <li>Type ({t.references} ActivityTypes)</li>
          </ul>
        </>
      )
    },
    {
      title: "ActivityImages",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> ActivityId ({t.references} Activities)</p>
        </>
      )
    },
    {
      title: "ActivityParticipantes",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>UserId ({t.references} UserInfos)</li>
            <li>ActivityId ({t.references} Activities)</li>
          </ul>
        </>
      )
    },
    {
      title: "ActivityReviews",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> ActivityReviewId</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>ActivityId ({t.references} Activities)</li>
            <li>UserId ({t.references} UserInfos)</li>
            <li>ReportItems ({t.references} ComplaintCategories)</li>
          </ul>
        </>
      )
    },
    {
      title: "ActivityTypes",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> {t.none}</p>
        </>
      )
    },
    {
      title: "Announcements",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> {t.none}</p>
        </>
      )
    },
    {
      title: "ChatRooms",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>User1Id ({t.references} UserInfos)</li>
            <li>User2Id ({t.references} UserInfos)</li>
          </ul>
        </>
      )
    },
    {
      title: "Comment",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>ParentCommentId ({t.references} Comment, nullable)</li>
            <li>PostId ({t.references} Posts)</li>
            <li>UserId ({t.references} UserInfos)</li>
          </ul>
        </>
      )
    },
    {
      title: "ComplaintCategories",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> ComplaintCategoryId</p>
          <p><strong>{t.foreignKeys}:</strong> {t.none}</p>
        </>
      )
    },
    {
      title: "ConId_UserId",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> UserId ({t.references} UserInfos)</p>
        </>
      )
    },
    {
      title: "FriendLists",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>Clicked ({t.references} UserInfos)</li>
            <li>BeenClicked ({t.references} UserInfos)</li>
          </ul>
        </>
      )
    },
    {
      title: "LabelCategories",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> {t.none}</p>
        </>
      )
    },
    {
      title: "Labels",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> CategoryId ({t.references} LabelCategories)</p>
        </>
      )
    },
    {
      title: "Likes",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>PostId ({t.references} Posts)</li>
            <li>UserId ({t.references} UserInfos)</li>
          </ul>
        </>
      )
    },
    {
      title: "ManagemenLogins",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> {t.none}</p>
        </>
      )
    },
    {
      title: "MemberLabels",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>UserId ({t.references} UserInfos)</li>
            <li>LabelId ({t.references} Labels)</li>
          </ul>
        </>
      )
    },
    {
      title: "PostReviews",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> PostReviewId</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>PostId ({t.references} Posts)</li>
            <li>UserId ({t.references} UserInfos)</li>
            <li>ReportItems ({t.references} ComplaintCategories)</li>
          </ul>
        </>
      )
    },
    {
      title: "Posts",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> UserId ({t.references} UserInfos)</p>
        </>
      )
    },
    {
      title: "SwipeHistories",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> UserId ({t.references} UserInfos)</p>
        </>
      )
    },
    {
      title: "Transactions",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> UserId ({t.references} UserInfos)</p>
        </>
      )
    },
    {
      title: "UserInfos",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> {t.none}</p>
        </>
      )
    },
    {
      title: "UserPictures",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong> UserId ({t.references} UserInfos)</p>
        </>
      )
    },
    {
      title: "UserReviews",
      content: (lang) => (
        <>
          <p><strong>{t.primaryKey}:</strong> Id</p>
          <p><strong>{t.foreignKeys}:</strong></p>
          <ul className="list-disc list-inside">
            <li>reportUserId ({t.references} UserInfos)</li>
            <li>beReportedUserId ({t.references} UserInfos)</li>
            <li>complaintCategoryId ({t.references} ComplaintCategories)</li>
          </ul>
        </>
      )
    }
  ];

  const nextSlide = () => {
    setCurrentSlide((prev) => (prev + 1) % slides.length);
  };

  const prevSlide = () => {
    setCurrentSlide((prev) => (prev - 1 + slides.length) % slides.length);
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
      <h1 className="text-3xl font-bold mb-6">{t.title}</h1>
      <div className="relative w-full max-w-2xl">
        <Slide {...slides[currentSlide]} lang={lang} />
        <div className="absolute top-1/2 left-0 transform -translate-y-1/2">
          <button onClick={prevSlide} className="bg-blue-500 text-white p-2 rounded-full">
            <ChevronLeft size={24} />
          </button>
        </div>
        <div className="absolute top-1/2 right-0 transform -translate-y-1/2">
          <button onClick={nextSlide} className="bg-blue-500 text-white p-2 rounded-full">
            <ChevronRight size={24} />
          </button>
        </div>
      </div>
      <p className="mt-4">
        {t.slide} {currentSlide + 1} {t.of} {slides.length}
      </p>
      <button onClick={toggleLang} className="mt-4 bg-green-500 text-white p-2 rounded-full">
        <Globe size={24} />
      </button>
    </div>
  );
};

export default Presentation;
