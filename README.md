# Gamified

## Game-Based Learning, Knowledge Validation & Achievement Platform

Gamified is an interactive learning and knowledge-validation platform that combines gameplay with educational challenges.

The core idea is simple:

**Learn → Play → Validate → Achieve**

Instead of relying only on certificates to demonstrate learning, Gamified aims to allow students to demonstrate their knowledge through interactive games and challenges. Successful performance can contribute to achievements and a personalized learning profile.

---

## 🚀 Project Vision

Gamified is envisioned as a learning ecosystem inspired by the idea of a professional digital profile.

Platforms such as LinkedIn allow users to build a professional identity through skills, projects, certificates and achievements.

Gamified aims to apply a similar concept to learning:

- Students learn concepts through educational resources.
- Students validate their understanding through interactive games.
- Performance contributes to achievements and progress.
- Achievements can become part of a personalized learning profile.

The current repository contains the working prototype of the game-based knowledge-validation component.

---

## 🎯 Problem Statement

Traditional assessments are often based on static question papers and repetitive practice.

Students may learn a concept but have limited opportunities to actively demonstrate and validate their understanding in an engaging environment.

Gamified aims to make knowledge validation more interactive by integrating questions and assessment directly into gameplay.

---

## 💡 Current Solution

The current prototype uses a Unity-based game environment where educational questions are integrated into gameplay.

The player:

1. Enters a question section.
2. Receives a question with multiple options.
3. Moves through the game environment.
4. Selects an answer through gameplay.
5. Receives immediate correct/false feedback.
6. Continues to the next question.
7. Completes the level after all available questions have been answered.

Questions are dynamically retrieved from Firebase rather than being hardcoded into the gameplay.

---

## ✨ Current Features

- Unity-based 3D gameplay
- Firebase Firestore question database
- Dynamic question retrieval
- Dynamic QuestionSection instantiation
- Multiple-choice questions
- Question numbering
- Correct/false answer validation
- Correct and incorrect audio feedback
- Delayed QuestionSection destruction
- Automatic progression between questions
- Automatic stopping when questions are exhausted
- Level completion mechanism
- Mobile movement controls
- Scalable question database structure

---

## 🏗️ System Architecture

```text
                  Firebase Firestore
                         │
                         │
                   Question Data
                         │
                         ▼
                  Unity Question
                     Manager
                         │
                         ▼
                Dynamic Question
                   Sections
                         │
                         ▼
                   Game Player
                         │
                         ▼
                 Answer Selection
                         │
                  ┌──────┴──────┐
                  ▼             ▼
               Correct        False
                  │             │
                  └──────┬──────┘
                         ▼
                   Feedback
                         │
                         ▼
                  Next Question
                         │
                         ▼
                  Level Complete
